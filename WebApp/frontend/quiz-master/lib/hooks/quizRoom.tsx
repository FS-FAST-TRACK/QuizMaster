import { QUIZMASTER_QUIZROOM_POST } from "@/api/api-routes";
import { CreateQuizRoom } from "../definitions/quizRoom";

export async function postQuizRoom(
    data: CreateQuizRoom,
    successCallback = () => {}
) {
    try {
        const token = localStorage.getItem("token"); //just temporary
        const res = await fetch(QUIZMASTER_QUIZROOM_POST, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify(data),
            credentials: "include",
        });
        successCallback();
        return {
            message: "Room created successfully.",
            data: res,
        };
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}
