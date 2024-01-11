import { useState } from "react";
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
    SystemInfoDto,
} from "./definitions";
import {
    QUIZMASTER_MEDIA_GET_DOWNLOAD,
    QUIZMASTER_QCATEGORY_GET_CATEGORIES,
    QUIZMASTER_QDIFFICULTY_GET_DIFFICULTIES,
    QUIZMASTER_QTYPE_GET_TYPES,
    QUIZMASTER_QUESTION_GET_QUESTION,
    QUIZMASTER_QUESTION_GET_QUESTIONS,
    QUIZMASTER_SET_GET_SET,
    QUIZMASTER_SET_GET_SETQUESTION,
    QUIZMASTER_SET_GET_SETQUESTIONS,
    QUIZMASTER_SET_GET_SETS,
} from "@/api/api-routes";

export async function fetchQuestions({
    questionResourceParameter,
}: {
    questionResourceParameter: QuestionResourceParameter;
}) {
    try {
        var apiUrl = `${QUIZMASTER_QUESTION_GET_QUESTIONS}?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`;
        if (
            questionResourceParameter.exludeQuestionsIds &&
            questionResourceParameter.exludeQuestionsIds.length !== 0
        ) {
            apiUrl = apiUrl.concat(
                `&exludeQuestionsIds=${JSON.stringify(
                    questionResourceParameter.exludeQuestionsIds
                )}`
            );
        }

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
        var apiUrl = `${QUIZMASTER_QCATEGORY_GET_CATEGORIES}`;
        if (questionResourceParameter) {
            apiUrl = apiUrl.concat(
                `?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`
            );
        }
        const data = await fetch(apiUrl).then(async (res) => {
            var data: QuestionCategory[];
            var paginationMetadata: PaginationMetadata | null;
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
        var apiUrl = `${QUIZMASTER_QDIFFICULTY_GET_DIFFICULTIES}`;
        if (difficultyResourceParameter) {
            apiUrl = apiUrl.concat(
                `?pageSize=${difficultyResourceParameter.pageSize}&pageNumber=${difficultyResourceParameter.pageNumber}&searchQuery=${difficultyResourceParameter.searchQuery}`
            );
        }
        const data = await fetch(apiUrl).then(async (res) => {
            var data: QuestionDifficulty[];
            var paginationMetadata: PaginationMetadata | null;
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
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch difficulties data.");
    }
}

export async function fetchTypes() {
    try {
        const data = await fetch(`${QUIZMASTER_QTYPE_GET_TYPES}`)
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
        var apiUrl = `${QUIZMASTER_QUESTION_GET_QUESTION}${questionId}`;

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
        const token = localStorage.getItem("token"); //just temporary
        var apiUrl = `${QUIZMASTER_SET_GET_SETS}`;
        const data = await fetch(apiUrl, {
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }).then(async (res) => {
            var data: Set[];
            data = await res.json();
            data.forEach((set) => {
                set.dateCreated = new Date(set.dateCreated);
                set.dateUpdated = new Date(set.dateUpdated);
            });

            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchSet({ setId }: { setId: number }) {
    try {
        const token = localStorage.getItem("token"); //just temporary
        var apiUrl = `${QUIZMASTER_SET_GET_SET}${setId}`;
        console.log(token);

        const data = await fetch(apiUrl, {
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }).then(async (res) => {
            var data: Set;
            data = await res.json();

            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchAllSetQuestions() {
    try {
        const token = localStorage.getItem("token"); //just temporary
        var apiUrl = `${QUIZMASTER_SET_GET_SETQUESTIONS}`;

        const data = await fetch(apiUrl, {
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }).then(async (res) => {
            var data: QuestionSet[];
            data = await res.json();
            data.forEach((set) => {
                set.dateCreated = new Date(set.dateCreated);
                set.dateUpdated = new Date(set.dateUpdated);
            });

            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchSetQuestions({ setId }: { setId: number }) {
    try {
        const token = localStorage.getItem("token"); //just temporary
        var apiUrl = `${QUIZMASTER_SET_GET_SETQUESTION}${setId}`;

        const data = await fetch(apiUrl, {
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }).then(async (res) => {
            var data: QuestionSet[];
            data = await res.json();
            data.forEach((set) => {
                set.dateCreated = new Date(set.dateCreated);
                set.dateUpdated = new Date(set.dateUpdated);
            });

            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchMedia(id: string) {
    try {
        const data = await fetch(`${QUIZMASTER_MEDIA_GET_DOWNLOAD}${id}`)
            .then(async (res) => {
                if (res.status === 404) {
                    throw new Error(`Media with id ${id} not found`);
                }
                return await res.blob();
            })
            .then((blob) => {
                var url = URL.createObjectURL(blob);
                return url;
            })
            .catch((error) => {
                console.warn(error);
                return null;
            });
        return { data };
    } catch (error) {
        throw new Error("Failed to fetch media.");
    }
}

export function fetchSystemInfo() {
    let systemInfo = {
        version: "1.0.0", 
        systemInfo:"Lorem ipsum dolor sit amet consectetur. Pulvinar porta egestas molestie purus faucibus neque malesuada lectus. Lacus auctor sit felis sed ultrices nullam sapien ornare justo. Proin adipiscing viverra vestibulum arcu sit. Suscipit bibendum ullamcorper ut et dolor quisque nulla et."
    };

    return systemInfo;
}