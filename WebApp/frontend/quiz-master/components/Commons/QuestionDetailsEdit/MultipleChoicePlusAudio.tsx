import { QuestionCreateValues, QuestionValues } from "@/lib/definitions";
import { InputLabel, Select, TextInput, Title } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";

export default function MultipleChoicePlusAudioQuestionDetail({
    form,
}: {
    form: UseFormReturnType<QuestionValues>;
}) {
    return (
        <div>
            <Title order={5}>Audio Translate</Title>
            <TextInput
                label="Text to Translate"
                variant="filled"
                withAsterisk
                {...form.getInputProps(
                    `questionDetailDtos.${form.values.questionDetailDtos.findIndex(
                        (detail) => detail.detailTypes.includes("textToAudio")
                    )}.qDetailDesc`
                )}
                onBlur={() => {
                    const qDetail = form.values.questionDetailDtos.find(
                        (qDetail) => qDetail.detailTypes.includes("textToAudio")
                    );
                    if (qDetail && qDetail?.qDetailDesc !== "") {
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
                }}
            />
            <Select
                variant="filled"
                label="Category"
                placeholder="Choose Category"
                className="w-64"
                data={[
                    { value: "ENG", label: "English" },
                    { value: "FIL", label: "Filipino" },
                    { value: "JAPAN", label: "Japanese" },
                ]}
                {...form.getInputProps(
                    `questionDetailDtos.${form.values.questionDetailDtos.findIndex(
                        (detail) => detail.detailTypes.includes("language")
                    )}.qDetailDesc`
                )}
                clearable
                required
                onChange={(value) => {
                    const qDetail = form.values.questionDetailDtos.find(
                        (qDetail) => qDetail.detailTypes.includes("language")
                    );
                    const qDetailIndex =
                        form.values.questionDetailDtos.findIndex((qDetail) =>
                            qDetail.detailTypes.includes("language")
                        );
                    if (qDetail && value) {
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
                        }).then(() => {
                            form.setFieldValue(
                                `questionDetailDtos.${qDetailIndex}.qDetailDesc`,
                                value
                            );
                        });
                    }
                }}
            />
            <MultipleChoiceQuestionDetail form={form} />
        </div>
    );
}
