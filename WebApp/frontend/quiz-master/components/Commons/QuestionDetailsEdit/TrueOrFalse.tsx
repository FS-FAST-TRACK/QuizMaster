import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionEdit,
    QuestionValues,
} from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { Checkbox, InputLabel, Text } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TrueOrFalseQuestionDetail({
    form,
}: {
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    const index = form.values.details.findIndex((qD) =>
        qD.detailTypes.includes("answer")
    );

    return (
        <div>
            <InputLabel>Choices</InputLabel>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-5 justify-start">
                <InputLabel htmlFor="answerIsTrue">
                    <div className="cursor-pointer flex h-[50px] px-5 py-3 gap-5 items-center border-[#D9D9D9] bg-white rounded-lg border-2">
                        <Checkbox
                            id="answerIsTrue"
                            size="sm"
                            radius="xl"
                            color="var(--primary)"
                            checked={
                                form.values.details
                                    .find((qDetail) =>
                                        qDetail.detailTypes.includes("answer")
                                    )
                                    ?.qDetailDesc.toLowerCase() === "true"
                            }
                            onChange={(e) => {
                                if (e.target.checked) {
                                    form.setFieldValue(
                                        `details.${index}.qDetailDesc`,
                                        "true"
                                    );
                                }
                            }}
                        />
                        <Text className="grow">True</Text>
                        {form.values.details.find((qDetail) =>
                            qDetail.detailTypes.includes("answer")
                        )?.qDetailDesc === "true" ? (
                            <Text
                                size="sm"
                                style={{
                                    color: "var(--primary)",
                                }}
                            >
                                Correct Answer
                            </Text>
                        ) : (
                            ""
                        )}
                    </div>
                </InputLabel>
                <InputLabel htmlFor="answerIsFalse">
                    <div className="cursor-pointer flex h-[50px] px-5 py-3 gap-5 items-center border-[#D9D9D9] bg-white rounded-lg border-2">
                        <Checkbox
                            id="answerIsFalse"
                            size="sm"
                            radius="xl"
                            color="var(--primary)"
                            checked={
                                form.values.details
                                    .find((qDetail) =>
                                        qDetail.detailTypes.includes("answer")
                                    )
                                    ?.qDetailDesc.toLocaleLowerCase() ===
                                "false"
                            }
                            onChange={(e) => {
                                if (e.target.checked) {
                                    form.setFieldValue(
                                        `details.${index}.qDetailDesc`,
                                        "false"
                                    );
                                }
                            }}
                        />
                        <Text className="grow">False</Text>
                        {form.values.details
                            .find((qDetail) =>
                                qDetail.detailTypes.includes("answer")
                            )
                            ?.qDetailDesc.toLowerCase() === "false" ? (
                            <Text
                                size="sm"
                                style={{
                                    color: "var(--primary)",
                                }}
                            >
                                Correct Answer
                            </Text>
                        ) : (
                            ""
                        )}
                    </div>
                </InputLabel>
            </div>
        </div>
    );
}
