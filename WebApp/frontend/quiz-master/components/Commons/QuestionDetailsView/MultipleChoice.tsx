"use client";

import { QuestionDetail } from "@/lib/definitions";
import { InputLabel, Text } from "@mantine/core";
import { CheckCircleIcon } from "@heroicons/react/24/outline";

export default function MultipleChoiceQuestionDetail({
    details,
}: {
    details?: QuestionDetail[];
}) {
    const optionFields = details?.map((item, index) => {
        if (!item.detailTypes.includes("option")) {
            return;
        }
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
                {isAnswer && <CheckCircleIcon className="w-6" />}

                <Text>{item.qDetailDesc}</Text>
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
    });

    return (
        <div>
            <InputLabel>Choices</InputLabel>
            <div className="mt-3 flex flex-col gap-2">{optionFields}</div>
        </div>
    );
}
