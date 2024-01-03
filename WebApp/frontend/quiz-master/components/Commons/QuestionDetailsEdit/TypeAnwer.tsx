import { QuestionCreateValues, QuestionValues } from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { InputLabel, TextInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TypeAnswerQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionValues>;
}) {
    const patchtDetail = (
        detailType:
            | "answer"
            | "option"
            | "minimum"
            | "maximum"
            | "language"
            | "interval"
            | "range"
            | "textToAudio"
    ) => {
        const qDetail = form.values.questionDetailDtos.find((qDetail) =>
            qDetail.detailTypes.includes(detailType)
        );
        const index = form.values.questionDetailDtos.findIndex((qDetail) =>
            qDetail.detailTypes.includes(detailType)
        );
        if (
            form.isValid(`questionDetailDtos.${index}.qDetailDesc`) &&
            qDetail &&
            qDetail?.qDetailDesc !== ""
        ) {
            patchQuestionDetail({
                questionId: form.values.id,
                id: qDetail.id,
                patchRequest: [
                    {
                        path: "/qDetailDesc",
                        op: "replace",
                        value: qDetail.qDetailDesc,
                    },
                ],
            });
        }
    };
    return (
        <div>
            <TextInput
                label="Short Anwer"
                variant="filled"
                withAsterisk
                {...form.getInputProps(
                    `questionDetailDtos.${form.values.questionDetailDtos.findIndex(
                        (detail) => detail.detailTypes.includes("answer")
                    )}.qDetailDesc`
                )}
                onBlur={() => {
                    patchtDetail("answer");
                }}
            />
        </div>
    );
}
