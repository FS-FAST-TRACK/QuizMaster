import {
    QUIZMASTER_QUESTION,
    QUIZMASTER_QUESTIONDETAIL_DELETE,
    QUIZMASTER_QUESTIONDETAIL_GET_QUESTIONDETAILS,
    QUIZMASTER_QUESTIONDETAIL_PATCH,
    QUIZMASTER_QUESTIONDETAIL_POST,
} from "@/api/api-routes";
import {
    PatchItem,
    QuestionDetail,
    QuestionDetailCreateDto,
} from "../definitions";
import { CustomResponse } from "@/api/definitions";

interface PatchQuestionDetailResponse extends CustomResponse {
    data: undefined;
}

interface MultipleQuestionDetailUpdateResponse extends CustomResponse {
    data: {
        id: number;
    };
}

export async function patchQuestionDetail({
    questionId,
    id,
    patchRequest,
}: {
    questionId: number;
    id: number;
    patchRequest: PatchItem[];
}) {
    try {
        const res = await fetch(
            `${QUIZMASTER_QUESTION}/${questionId}/${QUIZMASTER_QUESTIONDETAIL_PATCH}${id}`,
            {
                method: "PATCH",
                mode: "cors",
                credentials: "include",
                body: JSON.stringify(patchRequest),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
        var isSuccess = res.status < 300;
        const data = await res.json();
        var response: PatchQuestionDetailResponse = {
            statusCode: res.status,
            type: isSuccess ? "success" : "fail",
            message: isSuccess
                ? "Succesfuly update question detail. "
                : data.message,
            data: undefined,
        };
        return response;
    } catch (error) {
        throw new Error("Failed to update questionDetail");
    }
}

export async function fetchQuestionDetails({
    questionId,
}: {
    questionId: number;
}) {
    try {
        var apiUrl = `${QUIZMASTER_QUESTION}/${questionId}/${QUIZMASTER_QUESTIONDETAIL_GET_QUESTIONDETAILS}`;

        const response = await fetch(apiUrl, {
            credentials: "include",
        });
        const isSuccess = response.status < 300;

        var returnRes: CustomResponse = {
            statusCode: response.status,
            type: isSuccess ? "success" : "fail",
            message: "",
            data: [],
        };

        if (!isSuccess) {
            var error = await response.json();
            returnRes.message = error.message;
            return returnRes;
        }

        var data: QuestionDetail[];
        data = await response.json();
        returnRes.data = data;
        return returnRes;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question details.");
    }
}

export async function postQuestionDetail({
    questionId,
    questionDetail,
}: {
    questionId: number;
    questionDetail: QuestionDetailCreateDto;
}) {
    try {
        var data = fetch(
            `${QUIZMASTER_QUESTION}/${questionId}/${QUIZMASTER_QUESTIONDETAIL_POST}`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(questionDetail),
                headers: {
                    "Content-Type": "application/json",
                },
                credentials: "include",
            }
        ).then(async (res) => {
            var qDetail: QuestionDetail;
            qDetail = await res.json();
            return qDetail;
        });
        return data;
    } catch (error) {
        throw new Error("Failed to post Question Detail");
    }
}

export async function deleteQuestionDetail({
    questionId,
    id,
}: {
    questionId: number;
    id: number;
}) {
    try {
        fetch(
            `${QUIZMASTER_QUESTION}/${questionId}/${QUIZMASTER_QUESTIONDETAIL_DELETE}${id}`,
            {
                method: "DELETE",
                credentials: "include",
            }
        );
    } catch (error) {
        throw new Error("Failed to delete Question Detail");
    }
}

export async function getQuestionDetails({
    questionId,
}: {
    questionId: number;
}) {
    try {
        const { data } = await fetch(
            `${QUIZMASTER_QUESTION}/${questionId}/${QUIZMASTER_QUESTIONDETAIL_GET_QUESTIONDETAILS}`
        ).then(async (res) => {
            var data: QuestionDetail[];

            data = await res.json();

            return { data };
        });
        return data;
    } catch (error) {
        throw new Error("Failed to get Question Details");
    }
}
