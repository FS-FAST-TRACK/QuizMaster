import { QuestionCreateValues, QuestionValues } from "@/lib/definitions";
import { InputLabel, TextInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TypeAnswerQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionValues>;
}) {
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
            />
        </div>
    );
}
