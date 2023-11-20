"use client";

import {
    QuestionCategory,
    QuestionCreateValues,
    QuestionDifficulty,
    QuestionType,
} from "@/lib/definitions";
import { fetchCategories, fetchDifficulties, fetchTypes } from "@/lib/quizData";
import {
    Box,
    Button,
    Combobox,
    Input,
    InputLabel,
    Select,
    TextInput,
    Textarea,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { Children, useCallback, useEffect, useState } from "react";
import QuestionOption from "../QuestionOption";

export default function QuestionCreate() {
    const [categories, setCategories] = useState<QuestionCategory[]>([]);
    const [difficulties, setDifficulties] = useState<QuestionDifficulty[]>([]);
    const [types, setTypes] = useState<QuestionType[]>([]);

    useEffect(() => {
        fetchCategories().then((res) => {
            setCategories(res);
        });
        fetchDifficulties().then((res) => {
            setDifficulties(res);
        });
        fetchTypes().then((res) => {
            setTypes(res);
        });
    }, []);

    const form = useForm<QuestionCreateValues>({
        initialValues: {
            qAudio: "",
            qImage: "",
            qTime: "30",
            qStatement: "",
            qCategoryId: "1",
            qDifficultyId: "1",
            qTypeId: "1",
            questionDetailCreateDtos: [
                {
                    qDetailDesc: "",
                    detailTypes: ["option"],
                },
            ],
        },
        validate: {
            qStatement: (value) =>
                value.length < 2 ? "Invalid question statement." : null,
            qCategoryId: (value) =>
                parseInt(value) < 1 ? "Select Category." : null,
            qDifficultyId: (value) =>
                parseInt(value) < 1 ? "Select Difficulty." : null,
            qTypeId: (value) =>
                parseInt(value) < 1 ? "Select Question Type." : null,
            questionDetailCreateDtos: (value) => {
                var type = parseInt(form.values.qTypeId);
                var errors = "";
                switch (type) {
                    case 1:
                        if (
                            value.findIndex((qDetail) =>
                                qDetail.detailTypes.includes("answer")
                            ) < 0
                        ) {
                            errors +=
                                "Answer is required for multiple type of question. ";
                        }
                        if (
                            value.findIndex((qDetail) =>
                                qDetail.detailTypes.includes("option")
                            ) < 0
                        ) {
                            errors +=
                                "Option is required for multiple type of question. ";
                        }
                        break;
                    case 2:
                        if (
                            value.findIndex((qDetail) =>
                                qDetail.detailTypes.includes("answer")
                            ) < 0
                        ) {
                            errors +=
                                "Answer is required for multiple type of question. ";
                        }
                        if (
                            value.findIndex((qDetail) =>
                                qDetail.detailTypes.includes("option")
                            ) < 0
                        ) {
                            errors +=
                                "Option is required for multiple type of question. ";
                        }
                        break;
                }
                return errors == "" ? null : errors;
            },
        },
    });

    const handelSubmit = useCallback(() => {
        console.log(form.values);
    }, [form]);

    return (
        <div className="flex flex-col md:flex-row">
            <div className="flex flex-row flex-wrap grow gap-10">
                {/* Question presentation */}
                <div className="grow">
                    <div className="aspect-[16/9] min-w-[300px] h-auto p-10 rounded-3xl bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500">
                        {/* Question Statement */}
                        <Textarea
                            placeholder="Write Question"
                            size="xl"
                            radius="lg"
                            {...form.getInputProps("qStatement")}
                        />

                        {/* Question details (e.g. options, type answer, and puzzle) */}
                        <div>
                            <QuestionOption form={form} />
                        </div>
                    </div>
                </div>

                {/* Question Difficulty, Type, Catetorty */}
                <div className="flex flex-row flex-wrap md:flex-wrap-0 md:flex-col shrink gap-5 [&>*]:w-[200px]">
                    <Select
                        label="Category"
                        placeholder="Choose Category"
                        data={categories.map((cat) => {
                            return {
                                value: cat.id.toString(),
                                label: cat.qCategoryDesc,
                            };
                        })}
                        {...form.getInputProps("qCategoryId")}
                        clearable
                        required
                    />
                    <Select
                        label="Difficulty"
                        placeholder="Choose Difficulty"
                        data={difficulties.map((dif) => {
                            return {
                                value: dif.id.toString(),
                                label: dif.qDifficultyDesc,
                            };
                        })}
                        {...form.getInputProps("qDifficultyId")}
                        clearable
                        required
                    />
                    <Select
                        label="Question Type"
                        placeholder="Choose Question Type"
                        data={types.map((type) => {
                            return {
                                value: type.id.toString(),
                                label: type.qTypeDesc,
                            };
                        })}
                        {...form.getInputProps("qTypeId")}
                        clearable
                        required
                    />

                    <div className="flex flex-col justify-around ">
                        <InputLabel>Time Limit</InputLabel>
                        <Input
                            {...form.getInputProps("qTime")}
                            placeholder="Input component"
                            size="sm"
                            required
                        />
                    </div>
                    <div>
                        <Button onClick={handelSubmit}> Submit</Button>
                    </div>
                </div>
            </div>
        </div>
    );
}
