"use client";

import QuestionDetails from "@/components/Commons/QuestionDetails";
import {
    QuestionCreateDto,
    QuestionCreateValues,
    QuestionDetailCreateDto,
} from "@/lib/definitions";
import { mapData } from "@/lib/helpers";
import {
    MultipleChoiceData,
    MultipleChoicePlusAudioData,
    PuzzleData,
    SliderData,
} from "@/lib/questionTypeData";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { PhotoIcon, SpeakerWaveIcon } from "@heroicons/react/24/outline";
import {
    Anchor,
    Breadcrumbs,
    Button,
    InputLabel,
    Select,
    TextInput,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useRouter } from "next/navigation";
import { useCallback, useEffect, useState } from "react";

const timeLimits = [10, 30, 60, 120];

const items = [
    { label: "All", href: "/questions" },
    { label: "Create a Question", href: "#" },
    { label: "", href: "#" },
].map((item, index) => (
    <Anchor href={item.href} key={index}>
        <p className="text-black">{item.label}</p>
    </Anchor>
));

export default function Page() {
    const { questionCategories } = useQuestionCategoriesStore();
    const { questionDifficulties } = useQuestionDifficultiesStore();
    const { questionTypes } = useQuestionTypesStore();
    const router = useRouter();

    // const [categories, setCategories] = useState<QuestionCategory[]>([]);
    // const [difficulties, setDifficulties] = useState<QuestionDifficulty[]>([]);
    // const [types, setTypes] = useState<QuestionType[]>([]);

    // useEffect(() => {
    //     fetchCategories().then((res) => {
    //         setCategories(res);
    //     });
    //     fetchDifficulties().then((res) => {
    //         setDifficulties(res);
    //     });
    //     fetchTypes().then((res) => {
    //         setTypes(res);
    //     });
    // }, []);

    const form = useForm<QuestionCreateValues>({
        initialValues: {
            qStatement: "",
            qAudio: "",
            qImage: "",
            qCategoryId: "1",
            qDifficultyId: "1",
            qTypeId: "1",
            qTime: "30",
            questionDetailCreateDtos: [],
            options: [],
            trueOrFalseAnswer: true,
            minimum: 1,
            maximum: 1,
            sliderAnswer: 1,
            interval: 1,
            textToAudio: "",
            language: "ENG",
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            qStatement: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
            qCategoryId: (value, values) =>
                value?.length === 0 ? "Question Category is required." : null,
            interval: (value, values) => {
                if (parseInt(values.qTypeId) !== SliderData.id) {
                    return null;
                }
                if (!value) {
                    return "Provide interval";
                }
                return values.sliderAnswer &&
                    values.minimum &&
                    (values.sliderAnswer - values.minimum) % value !== 0
                    ? "Answer can't be hit with the given interval."
                    : null;
            },
            minimum: (value, values) => {
                if (parseInt(values.qTypeId) !== SliderData.id) {
                    return null;
                }
                if (!value) {
                    return "Provide minimum";
                }
                return values.maximum && value > values.maximum
                    ? "Minimum must not be larger than maximum"
                    : null;
            },
            maximum: (value, values) => {
                if (parseInt(values.qTypeId) !== SliderData.id) {
                    return null;
                }
                if (!value) {
                    return "Provide maximum";
                }
                return values.minimum && value < values.minimum
                    ? "Maximum must not be smaller than the minimum"
                    : null;
            },
            sliderAnswer: (value, values) => {
                if (parseInt(values.qTypeId) !== SliderData.id) {
                    return null;
                }
                if (!value) {
                    return "Provide answer";
                }
                if (values.minimum && values.minimum > value) {
                    return "Answer must not be smaller than the minimum.";
                }

                if (values.maximum && values.maximum < value) {
                    return "Answer must not be larger than the maximum.";
                }
                return values.minimum &&
                    values.interval &&
                    (value - values.minimum) % values.interval !== 0
                    ? "Answer cannot be hit with the given interval"
                    : null;
            },
            options: {
                value: (value, values, path) => {
                    if (
                        !value &&
                        (parseInt(values.qTypeId) === MultipleChoiceData.id ||
                            parseInt(values.qTypeId) ===
                                MultipleChoicePlusAudioData.id)
                    ) {
                        return "Provide option";
                    }

                    return values.options.findIndex((op, i) => {
                        return (
                            op.value === value && path !== `options.${i}.value`
                        );
                    }) >= 0
                        ? "Duplicated Choice"
                        : null;
                },
            },
        },
    });

    const handelSubmit = useCallback(async () => {
        const data = JSON.stringify(form.values);
        if (
            form.values.options.length === 0 &&
            (parseInt(form.values.qTypeId) === MultipleChoiceData.id ||
                parseInt(form.values.qTypeId) ===
                    MultipleChoicePlusAudioData.id ||
                parseInt(form.values.qTypeId) === PuzzleData.id)
        ) {
            form.setFieldError("options", "Provide options");
            return;
        }
        const questionCreateDto = mapData(form);
        console.log(questionCreateDto);
        const res = await fetch(`${process.env.QUIZMASTER_QUIZ}/api/question`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(questionCreateDto),
            headers: {
                "Content-Type": "application/json",
            },
        });

        console.log(res);
        if (res.status === 201) {
            router.push("/questions");
        }
    }, [form.values]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row justify-between text-2xl font-bold">
                <h3>Create New Question</h3>
            </div>
            <form
                className="flex flex-col gap-8"
                onSubmit={form.onSubmit((values) => {
                    console.log(values);
                    handelSubmit();
                })}
            >
                <TextInput
                    label="Statement"
                    variant="filled"
                    withAsterisk
                    {...form.getInputProps("qStatement")}
                />
                <div className="flex flex-col md:flex-row gap-5 md:gap-10 [&>*]:grow">
                    <Select
                        variant="filled"
                        label="Category"
                        placeholder="Choose Category"
                        data={questionCategories.map((cat) => {
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
                        variant="filled"
                        label="Difficulty"
                        placeholder="Choose Difficulty"
                        data={questionDifficulties.map((dif) => {
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
                        variant="filled"
                        label="Question Type"
                        placeholder="Choose Question Type"
                        data={questionTypes.map((type) => {
                            return {
                                value: type.id.toString(),
                                label: type.qTypeDesc,
                            };
                        })}
                        {...form.getInputProps("qTypeId")}
                        clearable
                        required
                    />
                </div>

                <Select
                    className="w-60"
                    variant="filled"
                    label="Time Limit"
                    placeholder="Choose Question Type"
                    data={timeLimits.map((time) => time.toString())}
                    {...form.getInputProps("qTime")}
                    clearable
                    required
                />

                <div>
                    <InputLabel>Media</InputLabel>
                    <div className="flex gap-4 justify-between sm:justify-start [&>*]:flex [&>*]:gap-4 [&>*]:border [&>*]:text-[#706E6D] [&>*]:bg-[#D9D9D9] [&>*]:px-4 [&>*]:py-3 [&>*]:rounded [&>*]:text-sm [&>*]:cursor-pointer">
                        <label htmlFor="question-image" className="">
                            <PhotoIcon className="w-5" />
                            <p>Insert Image</p>
                        </label>
                        <label htmlFor="question-audio" className="">
                            <SpeakerWaveIcon className="w-5" />
                            Insert Audio
                        </label>
                    </div>
                    <TextInput
                        id="question-image"
                        type="file"
                        className="hidden"
                    />
                    <TextInput
                        id="question-audio"
                        type="file"
                        className="hidden"
                    />
                </div>

                <QuestionDetails form={form} />

                <div className="flex justify-end">
                    <Button variant="transparent" color="gray">
                        Cancel
                    </Button>
                    <Button variant="filled" color="green" type="submit">
                        Create
                    </Button>
                </div>
            </form>
        </div>
    );
}
