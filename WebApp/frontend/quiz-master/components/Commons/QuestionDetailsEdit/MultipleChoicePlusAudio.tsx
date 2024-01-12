import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionEdit,
    QuestionValues,
} from "@/lib/definitions";
import { InputLabel, Select, TextInput, Title } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";

export default function MultipleChoicePlusAudioQuestionDetail({
    form,
}: {
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    const textToAudioIndex = form.values.details.findIndex((detail) =>
        detail.detailTypes.includes("textToAudio")
    );
    return (
        <div>
            <Title order={5}>Audio Translate</Title>
            <TextInput
                label="Text to Translate"
                variant="filled"
                withAsterisk
                {...form.getInputProps(
                    `details.${textToAudioIndex}.qDetailDesc`
                )}
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
                    `details.${form.values.details.findIndex((detail) =>
                        detail.detailTypes.includes("language")
                    )}.qDetailDesc`
                )}
                clearable
                required
            />
            <MultipleChoiceQuestionDetail form={form} />
        </div>
    );
}
