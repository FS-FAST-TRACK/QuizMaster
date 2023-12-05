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
    QuestionSet,
    Set,
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

export async function fetchSets() {
    try {
        var apiUrl = `${process.env.QUIZMASTER_GATEWAY}/gateway/api/set/all_set`;

        const data = await fetch(apiUrl).then(
            async (res) => {
                var data: Set[];
                data = await res.json();
                data.forEach((set) => {
                    set.dateCreated = new Date(set.dateCreated);
                    set.dateUpdated = new Date(set.dateUpdated);
                });

                return data;
            }
        );
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchSetQuestions() {
    try {
        var apiUrl = `${process.env.QUIZMASTER_GATEWAY}/gateway/api/set/all_question_set`;

        const data = await fetch(apiUrl).then(
            async (res) => {
                var data: QuestionSet[];
                data = await res.json();

                return data;
            }
        );
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchQuestionsInSet({ qSetId }: { qSetId: number }) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_GATEWAY}/gateway/api/set/get_question_set/${qSetId}`;

        const data = await fetch(apiUrl).then(
            async (res) => {
                var data: QuestionSet[];
                data = await res.json();

                return data;
            }
        );
        console.log(data);
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchMedia(id: string) {
    try {
        const data = await fetch(
            `${process.env.QUIZMASTER_MEDIA}/api/Media/download/${id}`
        )
            .then(async (res) => {
                return await res.blob();
            })
            .then((blob) => {
                var url = URL.createObjectURL(blob);
                return url;
            });
        return { data };
    } catch (error) { }
}
