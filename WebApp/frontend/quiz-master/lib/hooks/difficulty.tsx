import { CategoryCreateDto } from "@/components/Commons/modals/CreateCategoryModal";
import { PatchItem } from "../definitions";
import { DifficultyCreateDto } from "@/components/Commons/modals/CreateDifficultyModal";

export async function removeDifficulty({ id }: { id: number }) {
    try {
        const { res } = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty/${id}`,
            {
                method: "DELETE",
            }
        ).then((res) => {
            if (res.status > 300) {
                throw new Error("Failed to delete difficulty data.");
            }
            return { res };
        });

        return { res };
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
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(difficultyCreateDto),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        ).then((res) => {
            if (res.status !== 201) {
                throw new Error("Failed to create difficulty data.");
            }
            return res;
        });

        return { res };
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
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty/${id}`,
            {
                method: "PATCH",
                mode: "cors",
                body: JSON.stringify(patchRequest),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        ).then((res) => {
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
