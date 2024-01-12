import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionEdit,
    QuestionValues,
} from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { InputLabel, TextInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TypeAnswerQuestionDetails({
    form,
}: {
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    const answerFields = form.values.details.map((item, index) => {
        if (!item.detailTypes.includes("answer")) {
            return;
        }
        return (
            <TextInput
                label="Short Anwer"
                variant="filled"
                withAsterisk
                {...form.getInputProps(`details.${index}.qDetailDesc`)}
            />
        );
    });
    return <div>{answerFields}</div>;
}
