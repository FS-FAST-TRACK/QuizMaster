import { QuestionCreateValues } from "@/lib/definitions";
import { InputLabel, Select, TextInput, Title } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";

export default function MultipleChoicePlusAudioQuestionDetail({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
    return (
        <div>
            <Title order={5}>Audio Translate</Title>
            <TextInput
                label="Text to Translate"
                variant="filled"
                withAsterisk
                {...form.getInputProps("textToAudio")}
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
                {...form.getInputProps("language")}
                clearable
                required
            />
            <MultipleChoiceQuestionDetail form={form} />
        </div>
    );
}
