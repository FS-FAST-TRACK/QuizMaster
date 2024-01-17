"use client";

import { QuestionDetail } from "@/lib/definitions";
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
import styles from "./MultipleChoice.module.css";
import { notification } from "@/lib/notifications";

export default function MultipleChoiceQuestionDetail({
    form,
}: {
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    const optionFields = form.values.details.map((item, index) => {
        if (!item.detailTypes.includes("option")) {
            return;
        }
        return (
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
                            checked={item.detailTypes.includes("answer")}
                            onChange={(e) => {
                                if (e.target.checked) {
                                    form.setFieldValue(
                                        `details.${index}.detailTypes`,
                                        ["answer", "option"]
                                    );
                                } else {
                                    form.setFieldValue(
                                        `details.${index}.detailTypes`,
                                        ["option"]
                                    );
                                }
                            }}
                        />
                    }
                    rightSectionWidth={
                        item.detailTypes.includes("answer") ? 120 : 40
                    }
                    rightSection={
                        item.detailTypes.includes("answer") ? (
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
                                    onClick={() => {
                                        form.removeListItem("details", index);
                                    }}
                                />
                            </Tooltip>
                        )
                    }
                    leftSectionPointerEvents="visible"
                    rightSectionPointerEvents="visible"
                    {...form.getInputProps(`details.${index}.qDetailDesc`)}
                />
                <Input.Error>
                    {form.getInputProps(`details.${index}.qDetailDesc`).error}
                </Input.Error>
            </div>
        );
    });

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
                    onClick={() => {
                        if (
                            form.values.details.filter((qD) =>
                                qD.detailTypes.includes("option")
                            ).length >= 10
                        ) {
                            notification({
                                type: "info",
                                title: "Choices are limited up to 10.",
                                message: "Unable to add another choice",
                            });
                            return;
                        }
                        form.insertListItem("details", {
                            qDetailDesc: "",
                            detailTypes: ["option"],
                        });
                    }}
                >
                    <PlusCircleIcon className="w-6" />
                </Button>
            </div>
            <Input.Error>{form.getInputProps("details").error}</Input.Error>
        </div>
    );
}
