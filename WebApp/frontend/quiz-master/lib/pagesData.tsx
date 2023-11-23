type PageLink = {
    key: string;
    label: string;
    href: string;
};

export const DashboardPageData: PageLink = {
    key: "dashboard",
    label: "Dashboard",
    href: "/dashboard",
};

export const QuestionsPageData: PageLink = {
    key: "questions",
    label: "Questions",
    href: "/questions",
};

export const CreateQuestionPageData: PageLink = {
    key: "create-question",
    label: "Create Question",
    href: "/questions/create-question",
};

export const QuizRoomPageData: PageLink = {
    key: "quiz-room",
    label: "Quiz Room",
    href: "/questions/create-question",
};
