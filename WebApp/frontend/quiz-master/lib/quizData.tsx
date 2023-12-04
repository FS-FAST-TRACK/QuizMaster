import {
    PaginationMetadata,
    Question,
    QuestionCategory,
    QuestionDifficulty,
    QuestionResourceParameter,
    QuestionType,
    CategoryResourceParameter,
    DifficultyResourceParameter,
    QuestionDetail,
} from "./definitions";

export async function fetchQuestions({
    questionResourceParameter,
}: {
    questionResourceParameter: QuestionResourceParameter;
}) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`;

        const { data, paginationMetadata } = await fetch(apiUrl).then(
            async (res) => {
                var data: Question[];
                var paginationMetadata: PaginationMetadata;
                paginationMetadata = JSON.parse(
                    res.headers.get("x-pagination") || ""
                );
                data = await res.json();

                return { data, paginationMetadata };
            }
        );

        return { data, paginationMetadata };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchCategories(
    questionResourceParameter?: CategoryResourceParameter
) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question/category`;
        if (questionResourceParameter) {
            apiUrl = apiUrl.concat(
                `?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`
            );
        }
        const data = await fetch(apiUrl).then(async (res) => {
            var data: QuestionCategory[];
            var paginationMetadata: PaginationMetadata;
            paginationMetadata = JSON.parse(
                res.headers.get("x-pagination") || ""
            );
            data = await res.json();
            data.forEach((cat) => {
                cat.dateCreated = new Date(cat.dateCreated);
                cat.dateUpdated = new Date(cat.dateUpdated);
            });

            return { data, paginationMetadata };
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch categories data.");
    }
}

export async function fetchDifficulties(
    difficultyResourceParameter?: DifficultyResourceParameter
) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty`;
        if (difficultyResourceParameter) {
            apiUrl = apiUrl.concat(
                `?pageSize=${difficultyResourceParameter.pageSize}&pageNumber=${difficultyResourceParameter.pageNumber}&searchQuery=${difficultyResourceParameter.searchQuery}`
            );
        }
        const data = await fetch(apiUrl).then(async (res) => {
            var data: QuestionDifficulty[];
            var paginationMetadata: PaginationMetadata;
            paginationMetadata = JSON.parse(
                res.headers.get("x-pagination") || ""
            );
            data = await res.json();
            data.forEach((dif) => {
                dif.dateCreated = new Date(dif.dateCreated);
                dif.dateUpdated = new Date(dif.dateUpdated);
            });

            return { data, paginationMetadata };
        });

        console.log(data);
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch difficulties data.");
    }
}

export async function fetchTypes() {
    try {
        const data = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/type`
        )
            .then((res) => res.json())
            .then((data) => {
                var types: QuestionType[];
                types = data;
                return types;
            });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch types data.");
    }
}

export async function fetchQuestion({ questionId }: { questionId: number }) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question/${questionId}`;

        const { data } = await fetch(apiUrl).then(async (res) => {
            var data: Question;

            data = await res.json();

            return { data };
        });

        return { data };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchQuestionDetails({
    questionId,
}: {
    questionId: number;
}) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question/${questionId}/question-detail`;

        const { data } = await fetch(apiUrl).then(async (res) => {
            var data: QuestionDetail[];
            data = await res.json();
            return { data };
        });

        return { data };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question details.");
    }
}
