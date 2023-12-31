import {
    QUIZMASTER_QUESTION,
    QUIZMASTER_QUESTIONDETAIL_GET_QUESTIONDETAILS,
} from "@/api/api-routes";
import {
    PatchItem,
    QuestionDetail,
    QuestionDetailCreateDto,
} from "../definitions";

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
        await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/${questionId}/question-detail/${id}`,
            {
                method: "PATCH",
                mode: "cors",
                body: JSON.stringify(patchRequest),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
    } catch (error) {
        throw new Error("Failed to update questionDetail");
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
            `${process.env.QUIZMASTER_QUIZ}/api/question/${questionId}/question-detail`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(questionDetail),
                headers: {
                    "Content-Type": "application/json",
                },
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
            `${process.env.QUIZMASTER_QUIZ}/api/question/${questionId}/question-detail/${id}`,
            {
                method: "DELETE",
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
