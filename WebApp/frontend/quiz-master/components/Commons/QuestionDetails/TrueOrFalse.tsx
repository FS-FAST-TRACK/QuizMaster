import { QuestionCreateValues } from "@/lib/definitions";
import { Checkbox, InputLabel, Text } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function TrueOrFalseQuestionDetail({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
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
                            checked={form.values.trueOrFalseAnswer}
                            onChange={(e) => {
                                form.setFieldValue("trueOrFalseAnswer", true);
                            }}
                        />
                        <Text className="grow">True</Text>
                        {form.values.trueOrFalseAnswer ? (
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
                            checked={!form.values.trueOrFalseAnswer}
                            onChange={(e) => {
                                form.setFieldValue("trueOrFalseAnswer", false);
                            }}
                        />
                        <Text className="grow">False</Text>
                        {!form.values.trueOrFalseAnswer ? (
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
