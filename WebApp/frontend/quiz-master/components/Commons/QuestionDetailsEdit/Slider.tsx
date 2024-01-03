import { QuestionCreateValues, QuestionValues } from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { NumberInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function SliderQuestionDetails({
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
        <div className="flex gap-5">
            <NumberInput
                variant="filled"
                label="Min"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `questionDetailDtos.${form.values.questionDetailDtos.findIndex(
                        (detail) => detail.detailTypes.includes("minimum")
                    )}.qDetailDesc`
                )}
                onBlur={() => {
                    patchtDetail("minimum");
                }}
            />
            <NumberInput
                variant="filled"
                label="Max"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `questionDetailDtos.${form.values.questionDetailDtos.findIndex(
                        (detail) => detail.detailTypes.includes("maximum")
                    )}.qDetailDesc`
                )}
                onBlur={() => {
                    patchtDetail("maximum");
                }}
            />
            <NumberInput
                variant="filled"
                label="Interval"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `questionDetailDtos.${form.values.questionDetailDtos.findIndex(
                        (detail) => detail.detailTypes.includes("interval")
                    )}.qDetailDesc`
                )}
                onBlur={() => {
                    patchtDetail("interval");
                }}
            />
            <NumberInput
                variant="filled"
                label="Answer"
                withAsterisk
                hideControls
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
