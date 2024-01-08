import { CategoryCreateDto } from "@/components/Commons/modals/CreateCategoryModal";
import { PatchItem, QuestionCategory } from "../definitions";
import {
    QUIZMASTER_QCATEGORY_DELETE,
    QUIZMASTER_QCATEGORY_GET_CATEGORIES,
    QUIZMASTER_QCATEGORY_PATCH,
    QUIZMASTER_QCATEGORY_POST,
} from "@/api/api-routes";
interface GetAllCategoriesResponse {
    data: QuestionCategory[];
}
export async function getAllCategories() {
    try {
        var apiUrl = `${QUIZMASTER_QCATEGORY_GET_CATEGORIES}`;
        const response = await fetch(apiUrl);

        var data: QuestionCategory[];

        data = await response.json();
        data.forEach((cat) => {
            cat.dateCreated = new Date(cat.dateCreated);
            cat.dateUpdated = new Date(cat.dateUpdated);
        });

        var res: GetAllCategoriesResponse = {
            data: data,
        };
        return res;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch categories data.");
    }
}

export async function removeCategory({ id }: { id: number }) {
    try {
        const { res } = await fetch(`${QUIZMASTER_QCATEGORY_DELETE}/${id}`, {
            method: "DELETE",
            credentials: "include",
        }).then((res) => {
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
        const res = await fetch(`${QUIZMASTER_QCATEGORY_POST}`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(categoryCreateDto),
            headers: {
                "Content-Type": "application/json",
            },
            credentials: "include",
        }).then((res) => {
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
        const res = await fetch(`${QUIZMASTER_QCATEGORY_PATCH}${id}`, {
            method: "PATCH",
            mode: "cors",
            body: JSON.stringify(patchRequest),
            headers: {
                "Content-Type": "application/json",
            },
            credentials: "include",
        }).then((res) => {
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
