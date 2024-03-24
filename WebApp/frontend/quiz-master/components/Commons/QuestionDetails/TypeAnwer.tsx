import { QuestionCreateValues } from "@/lib/definitions";
import { InformationCircleIcon } from "@heroicons/react/24/outline";
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
            <div className="flex items-center gap-1 mt-2">
                <InformationCircleIcon
                    width={24}
                    height={24}
                    color="color: rgb(75 85 99)"
                />
                <p className="text-sm text-gray-600">
                    Note: If a question has multiple possible answers, enumerate
                    and seperate them with a <b className="animate-pulse">|</b>{" "}
                    symbol.
                </p>
            </div>
        </div>
    );
}
