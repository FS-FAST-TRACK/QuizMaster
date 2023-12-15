import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionValues,
} from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { CheckCircleIcon } from "@heroicons/react/24/outline";
import { InputLabel, Text, TextInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TypeAnswerQuestionDetails({
    details,
}: {
    details?: QuestionDetail[];
}) {
    const optionFields = details?.map((item, index) => {
        var isAnswer = item.detailTypes.includes("answer");
        return (
            <div
                key={index}
                className={`flex gap-3 border-2 ${
                    isAnswer
                        ? "border-[var(--primary)] text-[var(--primary)]"
                        : ""
                } p-2 items-center rounded-lg  font-semibold`}
            >
                <Text>{item.qDetailDesc}</Text>
                <div className="grow"></div>
                {isAnswer && (
                    <>
                        <CheckCircleIcon className="w-6 text-white bg-green-600 rounded-full" />
                        <Text
                            size="sm"
                            style={{
                                color: "var(--primary)",
                            }}
                        >
                            Correct Answer
                        </Text>
                    </>
                )}
            </div>
        );
    });

    return (
        <div>
            <InputLabel>Answer</InputLabel>
            <div className="mt-3 flex flex-col gap-2">{optionFields}</div>
        </div>
    );
}
