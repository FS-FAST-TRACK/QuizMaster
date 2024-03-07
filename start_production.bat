@echo off
color 0A
echo  _______  __   __  ___      ___        _______  _______  _______  ___      _______ 
echo ^|       ^|^|  ^| ^|  ^|^|   ^|    ^|   ^|      ^|       ^|^|       ^|^|   _   ^|^|   ^|    ^|       ^|
echo ^|    ___^|^|  ^| ^|  ^|^|   ^|    ^|   ^|      ^|  _____^|^|       ^|^|  ^|_^|  ^|^|   ^|    ^|    ___^|
echo ^|   ^|___ ^|  ^|_^|  ^|^|   ^|    ^|   ^|      ^| ^|_____ ^|       ^|^|       ^|^|   ^|    ^|   ^|___ 
echo ^|    ___^|^|       ^|^|   ^|___ ^|   ^|___   ^|_____  ^|^|      _^|^|       ^|^|   ^|___ ^|    ___^|
echo ^|   ^|    ^|       ^|^|       ^|^|       ^|   _____^| ^|^|     ^|_ ^|   _   ^|^|       ^|^|   ^|___ 
echo ^|___^|    ^|_______^|^|_______^|^|_______^|  ^|_______^|^|_______^|^|__^| ^|__^|^|_______^|^|_______^|                               
echo                           ----[QuizMaster System]----
echo.
echo.
echo Prepared by: Jayharron Mar Abejar
echo Make sure that environment file values are updated:
echo Update the following environment file(s):
echo 1 WebApp/.env (used by docker-compose-no-frontend.yml BACKEND)
echo 2 WebApp/frontend/quiz-master/.env (used by QuizMaster Admin FRONTEND)
echo 3 WebApp/frontend/quiz_session/.env (used by QuizMaster Session FRONTEND)
echo.
echo.
echo Initializing...
timeout /t 5 /nobreak > NUL
if exist "C:\Program Files\Docker\Docker\Docker Desktop.exe" (
  echo Starting Docker Desktop Service
  "C:\Program Files\Docker\Docker\Docker Desktop.exe"
  timeout /t 10 /nobreak > NUL
) else (
  echo Please install Docker Desktop
  echo https://docs.docker.com/desktop/install/windows-install/
  pause
  exit
)
cd "WebApp"
start cmd /k "docker-compose -f docker-compose-no-frontend.yml up --build"
cd "frontend/quiz-master" && start cmd /k "npm i && npm run build && npm run start -- --port 3000 --hostname=0.0.0.0"
cd "../.."
cd "frontend/quiz_session" && start cmd /k "npm i && npm run build && npm run start -- --port 3001 --hostname=0.0.0.0"
echo Starting Services... 
pause