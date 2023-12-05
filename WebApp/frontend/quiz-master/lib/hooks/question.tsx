import {
    PaginationMetadata,
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

export async function fetchQuestion({ questionId }: { questionId: number }) {
    try {
        var apiUrl = `${process.env.QUIZMASTER_QUIZ}/api/question/${questionId}`;

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
    image?: File;
    audio?: File;
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
        const res = await fetch(`${process.env.QUIZMASTER_QUIZ}/api/question`, {
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
