import {
    PaginationMetadata,
    Question,
    QuestionCategory,
    QuestionDifficulty,
    QuestionResourceParameter,
    QuestionType,
} from "./definitions";

export async function fetchQuestions({
    questionResourceParameter,
}: {
    questionResourceParameter: QuestionResourceParameter;
}) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`;

        const { data, paginationMetadata } = await fetch(apiUrl).then(
            async (res) => {
                var data: Question[];
                var paginationMetadata: PaginationMetadata;
                paginationMetadata = JSON.parse(
                    res.headers.get("x-pagination") || ""
                );
                data = await res.json();

                return { data, paginationMetadata };
            }
        );

        return { data, paginationMetadata };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchCategories() {
    try {
        const data = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category`
        )
            .then((res) => res.json())
            .then((data) => {
                var categories: QuestionCategory[];
                categories = data;
                categories.forEach((cat) => {
                    cat.dateCreated = new Date(cat.dateCreated);
                    cat.dateUpdated = new Date(cat.dateUpdated);
                });
                return categories;
            });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch categories data.");
    }
}

export async function fetchDifficulties() {
    try {
        const data = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty`
        )
            .then((res) => res.json())
            .then((data) => {
                var difficulties: QuestionDifficulty[];
                difficulties = data;
                return difficulties;
            });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch difficulties data.");
    }
}

export async function fetchTypes() {
    try {
        const data = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/type`
        )
            .then((res) => res.json())
            .then((data) => {
                var types: QuestionType[];
                types = data;
                return types;
            });
        return data;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch types data.");
    }
}
