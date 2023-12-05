import { PatchItem } from "../definitions";

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
        await fetch(`${process.env.QUIZMASTER_QUIZ}/api/question/${id}`, {
            method: "PATCH",
            mode: "cors",
            body: JSON.stringify(patchRequest),
            headers: {
                "Content-Type": "application/json",
            },
        });
    } catch (error) {
        throw new Error("Failed to update questionDetail");
    }
}
