export async function removeCategory({ id }: { id: number }) {
    try {
        const { res } = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category/${id}`
        ).then((res) => {
            if (res.status === 404) {
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
