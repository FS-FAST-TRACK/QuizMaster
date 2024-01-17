import { CategoryCreateDto } from "@/components/Commons/modals/CreateCategoryModal";
import {
    DifficultyResourceParameter,
    PaginationMetadata,
    PatchItem,
    QuestionDifficulty,
} from "../definitions";
import { DifficultyCreateDto } from "@/components/Commons/modals/CreateDifficultyModal";
import {
    QUIZMASTER_QDIFFICULTY_DELETE,
    QUIZMASTER_QDIFFICULTY_GET_DIFFICULTIES,
    QUIZMASTER_QDIFFICULTY_PATCH,
    QUIZMASTER_QDIFFICULTY_POST,
} from "@/api/api-routes";
import { CustomResponse } from "@/api/definitions";

interface GetAllDifficultiesResponse {
    data: QuestionDifficulty[];
}

export async function getAllDifficulties() {
    try {
        var apiUrl = `${QUIZMASTER_QDIFFICULTY_GET_DIFFICULTIES}?isGetAll=true`;

        const response = await fetch(apiUrl);

        var data: QuestionDifficulty[];
        data = await response.json();
        data.forEach((dif) => {
            dif.dateCreated = new Date(dif.dateCreated);
            dif.dateUpdated = new Date(dif.dateUpdated);
        });
        var res: GetAllDifficultiesResponse = {
            data: data,
        };
        return res;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch difficulties data.");
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

export async function removeDifficulty({ id }: { id: number }) {
    try {
        const response = await fetch(`${QUIZMASTER_QDIFFICULTY_DELETE}${id}`, {
            method: "DELETE",
            credentials: "include",
        });

        const isSuccess = response.status === 204;

        var res: CustomResponse = {
            statusCode: response.status,
            message: "Successfuly deleted",
            data: {},
            type: isSuccess ? "success" : "fail",
        };

        if (!isSuccess) {
            const data = await response.json();
            res.message = data.message;
        }

        return res;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to delete category data.");
    }
}

export async function createDifficulty({
    difficultyCreateDto,
}: {
    difficultyCreateDto: DifficultyCreateDto;
}) {
    try {
        const response = await fetch(`${QUIZMASTER_QDIFFICULTY_POST}`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(difficultyCreateDto),
            headers: {
                "Content-Type": "application/json",
            },
            credentials: "include",
        }).then((res) => {
            if (res.status !== 201) {
                throw new Error("Failed to create difficulty data.");
            }
            return res;
        });

        const isSuccess = response.status === 201 || response.status === 200;

        var res: CustomResponse = {
            statusCode: response.status,
            message: "Successfuly created.",
            data: {},
            type: isSuccess ? "success" : "fail",
        };

        if (!isSuccess) {
            const data = await response.json();
            res.message = data.message;
        }

        return res;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to create difficulty data.");
    }
}

export async function patchDifficulty({
    id,
    patchRequest,
}: {
    id: number;
    patchRequest: PatchItem[];
}) {
    try {
        const res = await fetch(`${QUIZMASTER_QDIFFICULTY_PATCH}${id}`, {
            method: "PATCH",
            mode: "cors",
            body: JSON.stringify(patchRequest),
            headers: {
                "Content-Type": "application/json",
            },
        }).then((res) => {
            if (res.status > 300) {
                throw new Error("Failed to update difficulty");
            }
            return res;
        });
        return { res };
    } catch (error) {
        throw new Error("Failed to update difficulty");
    }
}
