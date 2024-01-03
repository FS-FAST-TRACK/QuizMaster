import { QUIZMASTER_QUESTION_GET_QUESTION, QUIZMASTER_QUESTION_GET_QUESTIONS, QUIZMASTER_QUESTION_PATCH, QUIZMASTER_QUESTION_POST } from "@/api/api-routes";
import {
    PaginationMetadata,
    PatchItem,
    Question,
    QuestionCreateDto,
    QuestionResourceParameter,
} from "../definitions";

export async function fetchQuestions({
    questionResourceParameter,
}: {
    questionResourceParameter: QuestionResourceParameter;
}) {
    try {
        var apiUrl = `${QUIZMASTER_QUESTION_GET_QUESTIONS}?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`;
        if (questionResourceParameter.filterByCategories.length !== 0) {
            apiUrl = apiUrl.concat(
                `&filterByCategoriesId=${JSON.stringify(
                    questionResourceParameter.filterByCategories
                )}`
            );
        }
        if (questionResourceParameter.filterByTypes.length !== 0) {
            apiUrl = apiUrl.concat(
                `&filterByTypesId=${JSON.stringify(
                    questionResourceParameter.filterByTypes
                )}`
            );
        }
        if (questionResourceParameter.filterByDifficulties.length !== 0) {
            apiUrl = apiUrl.concat(
                `&filterByDifficultiesId=${JSON.stringify(
                    questionResourceParameter.filterByDifficulties
                )}`
            );
        }
        if(questionResourceParameter.exludeQuestionsIds && questionResourceParameter.exludeQuestionsIds.length !== 0){
            apiUrl = apiUrl.concat(
                `&exludeQuestionsIds=${JSON.stringify(
                    questionResourceParameter.exludeQuestionsIds
                )}`
            );
        }
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

export async function fetchQuestion({ questionId }: { questionId: number }) {
    try {
        var apiUrl = `${QUIZMASTER_QUESTION_GET_QUESTION}${questionId}`;

        const { data } = await fetch(apiUrl).then(async (res) => {
            var data: Question;

            data = await res.json();

            return { data };
        });

        return { data };
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function postQuestion({
    question,
    image,
    audio,
}: {
    question: QuestionCreateDto;
    image: File | undefined | null;
    audio: File | undefined | null;
}) {
    try {
        // Post Image
        if (image) {
            var imageForm = new FormData();
            imageForm.append("file", image);
            const imageRes = await fetch(
                `${process.env.QUIZMASTER_MEDIA}/api/media`,
                {
                    method: "POST",
                    mode: "cors",
                    body: imageForm,
                }
            );

            if (imageRes.ok) {
                // Parse the response body as JSON
                const responseBody = await imageRes.json();
                question.qImage = responseBody.fileInformation.id;
            } else {
                throw new Error("Failed Post Image.");
            }
        }

        // Post Audio
        if (audio) {
            var audioForm = new FormData();
            audioForm.append("file", audio);
            const audioRes = await fetch(
                `${process.env.QUIZMASTER_MEDIA}/api/media`,
                {
                    method: "POST",
                    mode: "cors",
                    body: audioForm,
                }
            );
            if (audioRes.ok) {
                // Parse the response body as JSON
                const responseBody = await audioRes.json();
                question.qAudio = responseBody.fileInformation.id;
            } else {
                throw new Error("Failed Post Audio.");
            }
        }

        // Post Question
        const res = await fetch(`${QUIZMASTER_QUESTION_POST}`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(question),
            headers: {
                "Content-Type": "application/json",
            },
        });

        if (res.status === 201) {
            return res;
        } else {
            throw new Error("Failed to create question");
        }
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}

export async function patchQuestion({
    id,
    patches,
    image,
    audio,
}: {
    id: number;
    patches: PatchItem[];
    image: File | undefined | null;
    audio: File | undefined | null;
}) {
    try {
        // Post Image
        if (image) {
            var imageForm = new FormData();
            imageForm.append("file", image);
            const imageRes = await fetch(
                `${process.env.QUIZMASTER_MEDIA}/api/media`,
                {
                    method: "POST",
                    mode: "cors",
                    body: imageForm,
                }
            );

            if (imageRes.ok) {
                // Parse the response body as JSON
                const responseBody = await imageRes.json();
                patches = patches.concat({
                    path: "qImage",
                    op: "replace",
                    value: responseBody.fileInformation.id,
                });
            } else {
                throw new Error("Failed Post Image.");
            }
        }

        // Post Audio
        if (audio) {
            var audioForm = new FormData();
            audioForm.append("file", audio);
            const audioRes = await fetch(
                `${process.env.QUIZMASTER_MEDIA}/api/media`,
                {
                    method: "POST",
                    mode: "cors",
                    body: audioForm,
                }
            );
            if (audioRes.ok) {
                // Parse the response body as JSON
                const responseBody = await audioRes.json();
                patches = patches.concat({
                    path: "qAudio",
                    op: "replace",
                    value: responseBody.fileInformation.id,
                });
            } else {
                throw new Error("Failed Post Audio.");
            }
        }

        // Post Question
        const res = await fetch(
            `${QUIZMASTER_QUESTION_PATCH}/${id}`,
            {
                method: "PATCH",
                mode: "cors",
                body: JSON.stringify(patches),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );

        if (res.status === 201) {
            var data: Question;

            data = await res.json();
            return data;
        } else {
            throw new Error("Failed to create question");
        }
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}
