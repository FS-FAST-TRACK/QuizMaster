import { QUIZMASTER_SET_POST, QUIZMASTER_SET_PUT } from "@/api/api-routes";
import { QuestionSetDTO, SetDTO } from "../definitions";

export async function postQuestionSet({
    questionSet
}: {
    questionSet: SetDTO;
}) {
    try {
        // Post Question
        const res = await fetch(
            `${QUIZMASTER_SET_POST}`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(questionSet),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
            console.log(res);
        if (res.status === 200) {
            return res;
        } else {
            throw new Error("Failed to create question");
        }
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}

export async function updateQuestionSet({
    id,
    questionSet,
}: {
    id: number;
    questionSet: SetDTO;
}) {
    try {
        // Post Question
        const res = await fetch(
            `${QUIZMASTER_SET_PUT}${id}`,
            {
                method: "PATCH",
                mode: "cors",
                body: JSON.stringify(questionSet),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );

        if (res.status === 200) {
            return res;
        } else {
            throw new Error("Failed to update question");
        }
    } catch (error) {
        throw new Error("Failed to update question.");
    }
}