import { QUIZMASTER_QTYPE_GET_TYPES } from "@/api/api-routes";
import { QuestionType } from "../definitions";

interface GetAllTypesResponse {
    data: QuestionType[];
}

export async function getAllTypes() {
    try {
        const response = await fetch(`${QUIZMASTER_QTYPE_GET_TYPES}`);
        var types: QuestionType[];
        types = await response.json();

        var res: GetAllTypesResponse = {
            data: types,
        };
        return res;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch types data.");
    }
}
