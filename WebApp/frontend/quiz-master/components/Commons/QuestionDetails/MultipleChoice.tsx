"use client";

import { QuestionCreateValues } from "@/lib/definitions";
import { PlusCircleIcon, TrashIcon } from "@heroicons/react/24/outline";
import {
    Button,
    Checkbox,
    Input,
    InputLabel,
    Text,
    Tooltip,
} from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import { useEffect, useState } from "react";
import styles from "./MultipleChoice.module.css";

export default function MultipleChoiceQuestionDetail({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
    const [fomr2, setForm2] = useState(form);

    useEffect(() => {
        setForm2(form);
    }, [form]);

    const optionFields = form.values.options.map((item, index) => (
        <div key={index} className="h-[60px]">
            <Input
                size="lg"
                itemProp="sadf"
                classNames={styles}
                style={{
                    flex: 1,
                }}
                leftSection={
                    <Checkbox
                        size="sm"
                        radius="xl"
                        color="var(--primary)"
                        checked={form.values.options[index].isAnswer}
                        {...form.getInputProps(`options.${index}.isAnswer`)}
                    />
                }
                rightSectionWidth={item.isAnswer ? 120 : 40}
                rightSection={
                    item.isAnswer ? (
                        <Text
                            size="sm"
                            style={{
                                color: "var(--primary)",
                            }}
                        >
                            Correct Answer
                        </Text>
                    ) : (
                        <Tooltip label="Remove">
                            <TrashIcon
                                className="w-6 cursor-pointer"
                                onClick={() =>
                                    form.removeListItem("options", index)
                                }
                            />
                        </Tooltip>
                    )
                }
                leftSectionPointerEvents="visible"
                rightSectionPointerEvents="visible"
                {...form.getInputProps(`options.${index}.value`)}
            />
            <Input.Error>
                {form.getInputProps(`options.${index}.value`).error}
            </Input.Error>
        </div>
    ));

    return (
        <div>
            <InputLabel>Choices</InputLabel>
            <div className="mt-5 grid grid-cols-1 md:grid-cols-2 gap-5">
                {optionFields}
                <Button
                    variant="outline"
                    color="gray"
                    size="lg"
                    className="border-4 outline-2 outline-gray-800"
                    onClick={() =>
                        form.insertListItem("options", {
                            value: "",
                            isAnswer: false,
                        })
                    }
                >
                    <PlusCircleIcon className="w-6" />
                </Button>
            </div>
            <Input.Error>{form.getInputProps("options").error}</Input.Error>
        </div>
    );
}
