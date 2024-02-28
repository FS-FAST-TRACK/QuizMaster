import {
    QUIZMASTER_QUIZROOM_GET,
    QUIZMASTER_QUIZROOM_POST,
} from "@/api/api-routes";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CreateQuizRoom } from "../definitions/quizRoom";

export const fetchQuizRooms = async () => {
    try {
        const token = localStorage.getItem("token"); //just temporary
        var apiUrl = `${QUIZMASTER_QUIZROOM_GET}`;
        const data = await fetch(apiUrl, {
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        });

        const rooms = await data.json();
        return rooms;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
};

export async function postQuizRoom(data: CreateQuizRoom, successCallback = () => {}) {
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

export async function updateQuizRoom() {}

export async function deleteQuizRoom() {}
