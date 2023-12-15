import { CategoryCreateDto } from "@/components/Commons/modals/CreateCategoryModal";
import { PatchItem } from "../definitions";

export async function removeCategory({ id }: { id: number }) {
    try {
        const { res } = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category/${id}`,
            {
                method: "DELETE",
            }
        ).then((res) => {
            if (res.status > 300) {
                throw new Error("Failed to Delete category data.");
            }
            return { res };
        });

        return { res };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to Delete category data.");
    }
}

export async function createCategory({
    categoryCreateDto,
}: {
    categoryCreateDto: CategoryCreateDto;
}) {
    try {
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(categoryCreateDto),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        ).then((res) => {
            if (res.status !== 201) {
                throw new Error("Failed to create category data.");
            }
            return res;
        });

        return { res };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to create category data.");
    }
}

export async function patchCategory({
    id,
    patchRequest,
}: {
    id: number;
    patchRequest: PatchItem[];
}) {
    try {
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category/${id}`,
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
                throw new Error("Failed to update category");
            }
            return res;
        });
        return { res };
    } catch (error) {
        throw new Error("Failed to update category");
    }
}
