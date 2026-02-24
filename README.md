# 🎯 QuizMaster

**QuizMaster** is a real-time, multiplayer quiz platform built on a microservices architecture. It supports live quiz sessions with WebSocket-based communication, role-based access control, media handling, and a full admin panel for managing questions, categories, and sessions.

### Vision

To ignite the spirit of friendly competition, knowledge exploration, and personal growth through the Quiz Bee Competition Web and Mobile Application, creating a community of lifelong learners and champions.

### Mission

At QuizMasters, our mission is to provide a dynamic and engaging platform for individuals of all ages and backgrounds to come together, test their knowledge, and celebrate learning. We strive to make the QuizMasters Web and Mobile Application a space where participants can challenge themselves, showcase their expertise, and learn from others. Through this platform, we aim to foster a sense of camaraderie, encourage intellectual development, and inspire a thirst for knowledge in an inclusive and accessible manner.

---

## 📐 Architecture Overview

QuizMaster is composed of multiple backend microservices, two frontend applications, a message broker, and a relational database — all orchestrated via Docker Compose.

```
┌─────────────────────────────────────────────────┐
│                   API Gateway                   │
│              (Port 5000 / 7081)                 │
└────────────────────┬────────────────────────────┘
                     │ gRPC / HTTP
    ┌────────────────┼─────────────────────┐
    │                │                     │
 Account          Auth API            Quiz API
 (5001/6001)   (5002/6002)          (5005/6005)
    │                │                     │
    └────────────────┼─────────────────────┘
                     │
       ┌─────────────┼─────────────┐
       │             │             │
  Media API    Monitoring     Session API
 (5003/6003)  (5004/6004)   (5006/6006)
                             │
                          RabbitMQ
                        (5672/15672)
                             │
                          MSSQL DB
                           (1433)
```

### Frontend Applications

| App                | Port | Description                                                        |
| ------------------ | ---- | ------------------------------------------------------------------ |
| QuizMaster Admin   | 3000 | Admin panel for managing quizzes, questions, users, and sessions   |
| QuizMaster Session | 3001 | Participant-facing interface for joining and playing quiz sessions |

---

## 🧩 Microservices

### Account API (`/api/account`)

Manages user accounts. Supports both full and partial account creation (partial accounts are prompted to complete their profile on first login).

| Method | Endpoint                      | Auth Required | Admin Required | Description                                      |
| ------ | ----------------------------- | :-----------: | :------------: | ------------------------------------------------ |
| POST   | `/api/account/create`         |       ✗       |       ✗        | Create a full user account                       |
| POST   | `/api/account/create_partial` |       ✗       |       ✗        | Create a partial account (email + username only) |
| GET    | `/api/account`                |       ✓       |       ✓        | List all accounts                                |
| GET    | `/api/account/{id}`           |       ✓       |       ✗        | Get account by ID                                |
| PATCH  | `/api/account/update/{id}`    |       ✓       |       ✗        | Update account details                           |
| DELETE | `/api/account/delete/{id}`    |       ✓       |       ✗        | Soft delete an account                           |

---

### Auth API (`/api/auth`)

Handles authentication via JWT stored in cookies. Admin privileges are encoded in the token.

| Method | Endpoint                   | Auth Required | Admin Required | Description                                      |
| ------ | -------------------------- | :-----------: | :------------: | ------------------------------------------------ |
| POST   | `/api/auth/login`          |       ✗       |       ✗        | Login with username/password, returns JWT cookie |
| POST   | `/api/auth/logout`         |       ✓       |       ✗        | Clear session cookie                             |
| POST   | `/api/auth/set_admin/{id}` |       ✓       |       ✓        | Grant or revoke admin privileges                 |
| GET    | `/api/auth/info`           |       ✓       |       ✗        | Get currently logged-in user info                |

---

### Quiz API (`/api/question`)

Core quiz management — questions, categories, difficulties, and types.

**Questions**

| Method | Endpoint             | Description                                 |
| ------ | -------------------- | ------------------------------------------- |
| POST   | `/api/question`      | Create a question (Admin)                   |
| PATCH  | `/api/question`      | Update a question (Admin)                   |
| GET    | `/api/question`      | List questions (public, supports filtering) |
| GET    | `/api/question/{id}` | Get question by ID (Admin)                  |
| DELETE | `/api/question/{id}` | Soft delete a question (Admin, owner only)  |

**Question Details** (`/api/question/{questionId}/question-detail`)
Full CRUD for question details (answers, options, etc.). Supported `detailTypes`: `answer`, `option`, `minimum`, `maximum`, `interval`, `textToAudio`, `language`.

**Categories** — `/api/question/category`
**Difficulties** — `/api/question/difficulty`
**Types** — `/api/question/type`

All category/difficulty/type endpoints require Admin + Authentication and support full CRUD.

---

### Quiz Session API (`/api/session`, `/api/room`)

Manages real-time quiz sessions and player interactions via REST + WebSocket.

| Method | Endpoint                       | Auth | Admin | Description                         |
| ------ | ------------------------------ | :--: | :---: | ----------------------------------- |
| POST   | `/api/session/create`          |  ✓   |   ✓   | Create a quiz room                  |
| PATCH  | `/api/session/update/{id}`     |  ✓   |   ✓   | Update session details              |
| DELETE | `/api/session/delete/{id}`     |  ✓   |   ✓   | Remove a session                    |
| POST   | `/api/session/start/{id}`      |  ✓   |   ✓   | Start the session                   |
| GET    | `/api/session/{id}`            |  ✓   |   ✓   | Get session details                 |
| GET    | `/api/session`                 |  ✓   |   ✓   | List all sessions                   |
| POST   | `/api/room/join/{ws-clientId}` |  ✓   |   ✗   | Join a room as a participant        |
| POST   | `/api/room/{id}/submit`        |  ✓   |   ✗   | Submit an answer                    |
| POST   | `/api/room/{id}/chat`          |  ✓   |   ✗   | Send a chat message (not persisted) |

**WebSocket** — `ws/quizmaster_ws`

| WS Event       | Description                                |
| -------------- | ------------------------------------------ |
| `notif`        | Player join/leave, countdown notifications |
| `chat`         | Real-time chat messages                    |
| `game_update`  | Quiz state changes, current question       |
| `leaderboard`  | Live scores and rankings                   |
| `round_set`    | Current question set info                  |
| `participants` | Real-time participant list                 |

---

### Media API (`/api/media`)

Handles file uploads (images, audio, video) used in quiz questions.

| Method | Endpoint                   | Description                       |
| ------ | -------------------------- | --------------------------------- |
| POST   | `/api/media`               | Upload a file (Admin)             |
| GET    | `/api/media`               | List all stored files (Admin)     |
| GET    | `/api/media/{id}`          | Get file metadata (Admin)         |
| GET    | `/api/media/download/{id}` | Download a file (Admin)           |
| DELETE | `/api/media/{id}`          | Delete a file permanently (Admin) |

---

### Monitoring API (`/api/logs`, `/api/status`)

Internal service health and audit logging.

| Method | Endpoint               | Description                               |
| ------ | ---------------------- | ----------------------------------------- |
| GET    | `/api/logs`            | View system logs (Admin)                  |
| GET    | `/api/status/{api_id}` | Health check per microservice (Admin)     |
| POST   | `/api/logs`            | Save a log entry (internal gRPC use only) |

---

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://docs.docker.com/desktop/install/windows-install/)
- Node.js (for running frontends without Docker)

### Environment Configuration

Before starting, update the following environment files:

| File                                | Used By                     |
| ----------------------------------- | --------------------------- |
| `WebApp/.env`                       | All backend Docker services |
| `WebApp/frontend/quiz-master/.env`  | Admin frontend              |
| `WebApp/frontend/quiz_session/.env` | Session frontend            |

Key variables to configure in `WebApp/.env`:

```env
HOST_IP=<your-host-ip>
HOST_PROTOCOL=http
BACKEND_ACCOUNT_CONN_STR=<mssql-connection-string>
BACKEND_QUIZ_CONN_STR=<mssql-connection-string>
BACKEND_SESSION_CONN_STR=<mssql-connection-string>
BACKEND_MEDIA_CONN_STR=<sqlite-connection-string>
BACKEND_MONITORING_CONN_STR=<mssql-connection-string>
GATEWAY_CORS=<allowed-origins>
NEXTAUTH_SECRET=<your-secret>
NEXTAUTH_URL=<your-nextauth-url>
```

### Running with Docker Compose

```bash
cd WebApp
docker-compose up --build
```

This starts all backend services, RabbitMQ, and MSSQL.

### Running Frontends Separately (Production)

Use the included `start_production.bat` on Windows, or manually:

```bash
# Admin Frontend (port 3000)
cd WebApp/frontend/quiz-master
npm i && npm run build && npm run start -- --port 3000 --hostname=0.0.0.0

# Session Frontend (port 3001)
cd WebApp/frontend/quiz_session
npm i && npm run build && npm run start -- --port 3001 --hostname=0.0.0.0
```

### Running Everything with Docker (including frontends)

```bash
cd WebApp
docker-compose -f docker-compose.yml up --build
```

---

## 🔌 Port Reference

| Service                | HTTP  | HTTPS |
| ---------------------- | ----- | ----- |
| API Gateway            | 5000  | 7081  |
| Account API            | 5001  | 6001  |
| Auth API               | 5002  | 6002  |
| Media API              | 5003  | 6003  |
| Monitoring API         | 5004  | 6004  |
| Quiz API               | 5005  | 6005  |
| Session API            | 5006  | 6006  |
| Admin Frontend         | 3000  | —     |
| Session Frontend       | 3001  | —     |
| RabbitMQ               | 5672  | —     |
| RabbitMQ Management UI | 15672 | —     |
| MSSQL                  | 1433  | —     |

---

## ⚙️ Quiz Session Options

When creating a session, the following options can be configured:

| Option                        | Values                    | Default  | Description                                  |
| ----------------------------- | ------------------------- | -------- | -------------------------------------------- |
| `Mode`                        | `elimination` \| `normal` | `normal` | Elimination mode advances top 50% each round |
| `Show_leaderboard_each_round` | `true` \| `false`         | `true`   | Show leaderboard after each round            |
| `Display_top_10_only`         | `true` \| `false`         | `true`   | Show only top 10 on leaderboard              |
| `Allow_reconnect`             | `true` \| `false`         | `true`   | Allow participants to reconnect              |
| `Allow_join_on_game_started`  | `true` \| `false`         | `false`  | Allow late joins after game starts           |

---

## 🛠 Tech Stack

- **Backend**: ASP.NET Core (C#), gRPC, RabbitMQ, SignalR (WebSocket)
- **Frontend**: Next.js (React)
- **Database**: Microsoft SQL Server 2022, SQLite (Media)
- **Infrastructure**: Docker, Docker Compose
- **Auth**: JWT (cookie-based)

---

## 👤 Author

**Jayharron Mar Abejar**  
Created: October 17, 2023

---

<!--

### OLD README.MD

# QuizMaster (Web and Mobile Application)

### Vision

To ignite the spirit of friendly competition, knowledge exploration, and personal growth through the Quiz Bee Competition Web and Mobile Application, creating a community of lifelong learners and champions.

### Mission

At QuizMasters, our mission is to provide a dynamic and engaging platform for individuals of all ages and backgrounds to come together, test their knowledge, and celebrate learning. We strive to make the QuizMasters Web and Mobile Application a space where participants can challenge themselves, showcase their expertise, and learn from others. Through this platform, we aim to foster a sense of camaraderie, encourage intellectual development, and inspire a thirst for knowledge in an inclusive and accessible manner.

## Build Status (Github Actions)

### Backend

| Microservices CLI                                                                                                                                                                                                                  | Microservices CLI                                                                                                                                                                                               |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![QuizMaster - Account API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.account.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.account.yml)              | [![QuizMaster - Auth API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.auth.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.auth.yml)    |
| [![QuizMaster - Monitoring API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.monitoring.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.monitoring.yml)     | [![QuizMaster - Quiz API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.quiz.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.quiz.yml)    |
| [![QuizMaster - Quiz Session API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.quizsession.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.quizsession.yml) | [![QuizMaster - Media API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.media.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.media.yml) |
| [![QuizMaster - Gateway API](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.gateway.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/backend.api.gateway.yml)              |

### Frontend

| Website CLI                                                                                                                                                                                                                                          | Game CLI                                                                                                                                                                                                                                        |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [![QuizMaster - Frontend/Website](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/frontend.api.quizmaster-frontend.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/frontend.api.quizmaster-frontend.yml) | [![QuizMaster - Frontend/Game](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/frontend.api.quizmaster-session.yml/badge.svg)](https://github.com/FS-FAST-TRACK/QuizMaster/actions/workflows/frontend.api.quizmaster-session.yml) |

### Technology Stacks

| Backend       | Frontend(Web) | Mobile       |
| ------------- | ------------- | ------------ |
| C#            | Next.js       | React-Native |
| ASP.NET Core  | TailwindCSS   | NativeWind   |
| Microservices | Zustand       | Zustand      |
| API           | NextAuth      |
| RabbitMQ      |
| gRPC          |

-->
