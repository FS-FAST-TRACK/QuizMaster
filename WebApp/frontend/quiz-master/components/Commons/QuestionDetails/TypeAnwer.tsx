import { QuestionCreateValues } from "@/lib/definitions";
import { InputLabel, TextInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TypeAnswerQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
    return (
        <div>
            <TextInput
                label="Short Anwer"
                variant="filled"
                withAsterisk
                {...form.getInputProps("typeAnswer")}
            />
        </div>
    );
}
