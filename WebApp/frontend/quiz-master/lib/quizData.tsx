import { useState } from "react";
import {
    PaginationMetadata,
    Question,
    QuestionCategory,
    CategoryResourceParameter,
    QuestionDetail,
    QuestionSet,
    Set,

    SystemInfoDto,
    UserInfo,
    ContactDetails,
    Review,

} from "./definitions";
import {
    QUIZMASTER_AUTH_GET_COOKIE_INFO,
    QUIZMASTER_MEDIA_GET_DOWNLOAD,
    QUIZMASTER_QCATEGORY_GET_CATEGORIES,
    QUIZMASTER_QUESTION_GET_QUESTION,
    QUIZMASTER_SET_GET_SET,
    QUIZMASTER_SET_GET_SETQUESTION,
    QUIZMASTER_SET_GET_SETQUESTIONS,
    QUIZMASTER_SET_GET_SETS,
    QUIZMASTER_SYSTEM_ADMIN_GET_REVIEW,
    QUIZMASTER_SYSTEM_GET_CONTACT_INFO,
    QUIZMASTER_SYSTEM_GET_SYSTEM_INFO,
    QUIZMASTER_SYSTEM_USER_GET_REVIEW,
} from "@/api/api-routes";

export async function fetchLoginUser() {
    try {
        const token = localStorage.getItem("token"); //just temporary
        var apiUrl = `${QUIZMASTER_AUTH_GET_COOKIE_INFO}`;

        const data = await fetch(apiUrl, {
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }).then(async (res) => {
            var data: UserInfo;
            const resJson = await res.json();
            if(resJson?.status === 401) {
                return null;
            }
            data = resJson;
            return data;
        }).catch(e => {
            console.error("unauthorized", e);
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch user data");
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
        const data = await fetch(`${QUIZMASTER_MEDIA_GET_DOWNLOAD}${id}`, {
            credentials: "include",
        })
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
export async function fetchSystemInfo() {

    try {
        var apiUrl = `${QUIZMASTER_SYSTEM_GET_SYSTEM_INFO}`;

        const data = await fetch(apiUrl, {
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
            },
        }).then(async (res) => {
            var data: SystemInfoDto;
            const resJson = await res.json();
            data = resJson.data;
            
            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch system info.");
    }
}

export async function fetchContactInfo() {

    try {
        var apiUrl = `${QUIZMASTER_SYSTEM_GET_CONTACT_INFO}`;

        const data = await fetch(apiUrl, {
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
            },
        }).then(async (res) => {
            var data: ContactDetails;
            const resJson = await res.json();
            data = resJson.data;
            
            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch contact info.");
    }
}

export async function fetchReviewsForAdmin() {
    try {
        const token = localStorage.getItem("token");
        var apiUrl = `${QUIZMASTER_SYSTEM_ADMIN_GET_REVIEW}`;

        const data = await fetch(apiUrl, {
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        }).then(async (res) => {
            var data: Review[];
            const resJson = await res.json();
            data = resJson.data;
            
            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch reviews.");
    }
}

export async function fetchReviewsForClient() {
    try {
        var apiUrl = `${QUIZMASTER_SYSTEM_USER_GET_REVIEW}`;

        const data = await fetch(apiUrl, {
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
            },
        }).then(async (res) => {
            var data: Review[];
            const resJson = await res.json();
            data = resJson.data;
            
            return data;
        });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch reviews.");
    }
}