import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionValues,
} from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { CheckCircleIcon } from "@heroicons/react/24/outline";
import { Checkbox, InputLabel, Text } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TrueOrFalseQuestionDetail({
    details,
}: {
    details?: QuestionDetail[];
}) {
    var isTrue =
        details?.find((qdetail) => qdetail.detailTypes.includes("answer"))
            ?.qDetailDesc === "true";

    const optionField = (value: string, isAnswer: boolean) => (
        <div
            className={`flex gap-3 border-2 ${
                isAnswer ? "border-[var(--primary)] text-[var(--primary)]" : ""
            } p-2 items-center h-[50px] rounded-lg  font-bold`}
        >
            {isAnswer && <CheckCircleIcon className="w-6" />}

            <Text>{value}</Text>
            <div className="grow"></div>
            {isAnswer && (
                <Text
                    size="sm"
                    style={{
                        color: "var(--primary)",
                    }}
                >
                    Correct Answer
                </Text>
            )}
        </div>
    );

    return (
        <div>
            <InputLabel>Choices</InputLabel>
            <div className="mt-5 flex flex-col gap-2">
                {optionField("True", isTrue)}
                {optionField("False", !isTrue)}
            </div>
        </div>
    );
}
