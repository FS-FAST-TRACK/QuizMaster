import {
    QUIZMASTER_MEDIA_POST,
    QUIZMASTER_QUESTION_DELETE,
    QUIZMASTER_QUESTION_GET_QUESTION,
    QUIZMASTER_QUESTION_GET_QUESTIONS,
    QUIZMASTER_QUESTION_PATCH,
    QUIZMASTER_QUESTION_POST,
} from "@/api/api-routes";
import {
    PaginationMetadata,
    PatchItem,
    Question,
    QuestionCreateDto,
    QuestionResourceParameter,
} from "../definitions";
import { CustomResponse } from "@/api/definitions";

interface QuestionPostResponse extends CustomResponse {
    data: Question | undefined;
}

interface QuestionsGetResponse extends CustomResponse {
    data:
        | {
              questions: Question[];
              paginationMetada: PaginationMetadata;
          }
        | undefined;
}

interface QuestionGetResponse extends CustomResponse {
    data:
        | {
              question: Question;
          }
        | undefined;
}

export async function fetchQuestions({
    questionResourceParameter,
}: {
    questionResourceParameter: QuestionResourceParameter;
}) {
    try {
        var apiUrl = `${QUIZMASTER_QUESTION_GET_QUESTIONS}?pageSize=${questionResourceParameter.pageSize}&pageNumber=${questionResourceParameter.pageNumber}&searchQuery=${questionResourceParameter.searchQuery}`;

        // Add filter by categories
        if (questionResourceParameter.filterByCategories.length !== 0) {
            apiUrl = apiUrl.concat(
                `&filterByCategoriesId=${JSON.stringify(
                    questionResourceParameter.filterByCategories
                )}`
            );
        }

        // Add filter by types
        if (questionResourceParameter.filterByTypes.length !== 0) {
            apiUrl = apiUrl.concat(
                `&filterByTypesId=${JSON.stringify(
                    questionResourceParameter.filterByTypes
                )}`
            );
        }

        // Add filter by difficulties
        if (questionResourceParameter.filterByDifficulties.length !== 0) {
            apiUrl = apiUrl.concat(
                `&filterByDifficultiesId=${JSON.stringify(
                    questionResourceParameter.filterByDifficulties
                )}`
            );
        }

        // Add filter excluding some questions given their ids
        if (
            questionResourceParameter.exludeQuestionsIds &&
            questionResourceParameter.exludeQuestionsIds.length !== 0
        ) {
            apiUrl = apiUrl.concat(
                `&exludeQuestionsIds=${JSON.stringify(
                    questionResourceParameter.exludeQuestionsIds
                )}`
            );
        }

        const res = await fetch(apiUrl);

        const isSuccess = res.status === 201 || res.status === 200;
        var response: QuestionsGetResponse = {
            statusCode: res.status,
            type: isSuccess ? "success" : "fail",
            message: "",
            data: undefined,
        };

        if (!isSuccess) {
            var error = await res.json();
            response.message = error.Message;
            return response;
        }

        var data: Question[];
        var paginationMetadata: PaginationMetadata;
        paginationMetadata = JSON.parse(res.headers.get("x-pagination") || "");
        data = await res.json();

        response.data = {
            questions: data,
            paginationMetada: paginationMetadata,
        };
        return response;
    } catch (error) {
        console.error("Database Error:", error);
        throw new Error("Failed to fetch question data.");
    }
}

export async function fetchQuestion({ questionId }: { questionId: number }) {
    try {
        var apiUrl = `${QUIZMASTER_QUESTION_GET_QUESTION}${questionId}`;

        const res = await fetch(apiUrl);
        const response: QuestionGetResponse = {
            statusCode: res.status,
            type: "success",
            data: undefined,
            message: "",
        };
        const isSuccess = res.status < 300;
        if (isSuccess) {
            var data: Question;
            data = await res.json();
            response.data = { question: data };
        } else {
            response.type = "fail";
        }

        return response;
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
        var response: QuestionPostResponse = {
            statusCode: 200,
            type: "success",
            message: "",
            data: undefined,
        };

        // Post Image
        if (image) {
            var imageForm = new FormData();
            imageForm.append("file", image);
            const imageRes = await fetch(`${QUIZMASTER_MEDIA_POST}`, {
                method: "POST",
                mode: "cors",
                body: imageForm,
                credentials: "include",
            });

            if (imageRes.ok) {
                // Parse the response body as JSON
                const responseBody = await imageRes.json();
                question.qImage = responseBody.fileInformation.id;
            } else {
                const data = await imageRes.json();
                response.statusCode = imageRes.status;
                response.type = "fail";
                response.message = data.Message;
                return response;
            }
        }

        // Post Audio
        if (audio) {
            var audioForm = new FormData();
            audioForm.append("file", audio);
            const audioRes = await fetch(`${QUIZMASTER_MEDIA_POST}`, {
                method: "POST",
                mode: "cors",
                body: audioForm,
                credentials: "include",
            });
            if (audioRes.ok) {
                // Parse the response body as JSON
                const responseBody = await audioRes.json();
                question.qAudio = responseBody.fileInformation.id;
            } else {
                const data = await audioRes.json();
                response.statusCode = audioRes.status;
                response.type = "fail";
                response.message = data.Message;
                return response;
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
            credentials: "include",
        });

        const isSuccess = res.status === 201 || res.status === 200;

        if (!isSuccess) {
            const data = await res.json();
            response.message = data.message;

            return response;
        }
        const questionData: Question = await res.json();
        response.data = questionData;
        response.message = "Question added succesfully.";
        return response;
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
        var response: QuestionPostResponse = {
            statusCode: 200,
            type: "success",
            message: "",
            data: undefined,
        };

        // Post Image
        if (image) {
            var imageForm = new FormData();
            imageForm.append("file", image);
            const imageRes = await fetch(`${QUIZMASTER_MEDIA_POST}`, {
                method: "POST",
                mode: "cors",
                credentials: "include",
                body: imageForm,
            });

            if (imageRes.ok) {
                // Parse the response body as JSON
                const responseBody = await imageRes.json();
                patches = patches.concat({
                    path: "qImage",
                    op: "replace",
                    value: responseBody.fileInformation.id,
                });
            } else {
                const data = await imageRes.json();
                response.statusCode = imageRes.status;
                response.type = "fail";
                response.message = data.Message;
                return response;
            }
        }

        // Post Audio
        if (audio) {
            var audioForm = new FormData();
            audioForm.append("file", audio);
            const audioRes = await fetch(`${QUIZMASTER_MEDIA_POST}`, {
                method: "POST",
                mode: "cors",
                credentials: "include",
                body: audioForm,
            });
            if (audioRes.ok) {
                // Parse the response body as JSON
                const responseBody = await audioRes.json();
                patches = patches.concat({
                    path: "qAudio",
                    op: "replace",
                    value: responseBody.fileInformation.id,
                });
            } else {
                const data = await audioRes.json();
                response.statusCode = audioRes.status;
                response.type = "fail";
                response.message = data.Message;
                return response;
            }
        }

        // Post Question
        const res = await fetch(`${QUIZMASTER_QUESTION_PATCH}${id}`, {
            method: "PATCH",
            mode: "cors",
            credentials: "include",
            body: JSON.stringify(patches),
            headers: {
                "Content-Type": "application/json",
            },
        });
        const isSuccess = res.status === 201 || res.status === 200;

        if (!isSuccess) {
            const data = await res.json();
            response.message = data.message;
            response.type = "fail";
            return response;
        }
        const questionData: Question = await res.json();
        response.data = questionData;
        return response;
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}

export async function deleteQuestion({ id }: { id: number }) {
    try {
        // Post Question
        const res = await fetch(`${QUIZMASTER_QUESTION_DELETE}${id}`, {
            mode: "cors",
            method: "DELETE",
            credentials: "include",
        });

        const isSuccess = res.status === 200 || res.status === 204;

        var response: CustomResponse = {
            statusCode: res.status,
            type: isSuccess ? "success" : "fail",
            message: isSuccess ? "Question succesfully deleted." : "",
            data: null,
        };
        if (!isSuccess) {
            var data = await res.json();
            response.message = data.message;
        }

        return response;
    } catch (error) {
        throw new Error("Failed to delete question.");
    }
}
