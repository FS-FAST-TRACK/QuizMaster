"use client";

import QuestionDetailsEdit from "@/components/Commons/QuestionDetailsEdit";
import { QuestionValues } from "@/lib/definitions";
import { humanFileSize } from "@/lib/helpers";
import {
    MultipleChoiceData,
    MultipleChoicePlusAudioData,
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
    FileInput,
    InputLabel,
    LoadingOverlay,
    Select,
    TextInput,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { useRouter } from "next/navigation";
import { useCallback, useEffect, useState } from "react";
import styles from "@/styles/input.module.css";
import { fetchQuestion } from "@/lib/quizData";

const timeLimits = [10, 30, 60, 120];

const items = [
    { label: "All", href: "/questions" },
    { label: "Edit Question", href: "#" },
    { label: "", href: "#" },
].map((item, index) => (
    <Anchor href={item.href} key={index}>
        <p className="text-black">{item.label}</p>
    </Anchor>
));

export default function Page({ params }: { params: { id: number } }) {
    const { questionCategories } = useQuestionCategoriesStore();
    const { questionDifficulties } = useQuestionDifficultiesStore();
    const { questionTypes } = useQuestionTypesStore();
    const router = useRouter();
    const [visible, { toggle, close }] = useDisclosure(false);
    const [fileImage, setFileImage] = useState<File | null>(null);
    const [fileAudio, setFileAudio] = useState<File | null>(null);
    const [isFetching, setIsFetching] = useState<boolean>(false);

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
    useEffect(() => {
        console.log(params.id);
        fetchQuestion({ questionId: params.id })
            .then((data) => {
                console.log(data);
                form.setInitialValues({
                    id: data.data.id,
                    qStatement: data.data.qStatement,
                    qAudio: data.data.qAudio,
                    qImage: data.data.qImage,
                    qCategoryId: data.data.qCategoryId.toString(),
                    qDifficultyId: data.data.qDifficultyId.toString(),
                    qTypeId: data.data.qTypeId.toString(),
                    qTime: data.data.qTime.toString(),
                    questionDetailCreateDtos: [],
                    questionDetailDtos: data.data.details,
                    options: [],
                    trueOrFalseAnswer: true,
                    minimum: 1,
                    maximum: 1,
                    sliderAnswer: 1,
                    interval: 1,
                    textToAudio: "",
                    language: "ENG",
                });
                form.setValues({
                    id: data.data.id,
                    qStatement: data.data.qStatement,
                    qAudio: data.data.qAudio,
                    qImage: data.data.qImage,
                    qCategoryId: data.data.qCategoryId.toString(),
                    qDifficultyId: data.data.qDifficultyId.toString(),
                    qTypeId: data.data.qTypeId.toString(),
                    qTime: data.data.qTime.toString(),
                    questionDetailCreateDtos: [],
                    questionDetailDtos: data.data.details,
                    options: [],
                    trueOrFalseAnswer: true,
                    minimum: 1,
                    maximum: 1,
                    sliderAnswer: 1,
                    interval: 1,
                    textToAudio: "",
                    language: "ENG",
                });
            })
            .catch(() => {});
    }, [params.id]);

    const form = useForm<QuestionValues>({
        initialValues: {
            id: 1,
            qStatement: "",
            qAudio: "",
            qImage: "",
            qCategoryId: "1",
            qDifficultyId: "1",
            qTypeId: "1",
            qTime: "30",
            questionDetailCreateDtos: [],
            questionDetailDtos: [],
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
            questionDetailDtos: {
                qDetailDesc: (value, values, path) => {
                    if (!value) {
                        return "Provide content.";
                    }

                    if (
                        parseInt(values.qTypeId) === MultipleChoiceData.id ||
                        parseInt(values.qTypeId) ===
                            MultipleChoicePlusAudioData.id
                    ) {
                        return values.questionDetailDtos.findIndex((op, i) => {
                            return (
                                op.qDetailDesc === value &&
                                path !== `questionDetailDtos.${i}.qDetailDesc`
                            );
                        }) >= 0
                            ? "Duplicated Choice"
                            : null;
                    }

                    if (parseInt(values.qTypeId) === SliderData.id) {
                        var min = values.questionDetailDtos.find((qDetail) =>
                            qDetail.detailTypes.includes("minimum")
                        );
                        var max = values.questionDetailDtos.find((qDetail) =>
                            qDetail.detailTypes.includes("maximum")
                        );
                        var interval = values.questionDetailDtos.find(
                            (qDetail) =>
                                qDetail.detailTypes.includes("interval")
                        );
                        var answer = values.questionDetailDtos.find((qDetail) =>
                            qDetail.detailTypes.includes("answer")
                        );

                        if (
                            values.questionDetailDtos.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("minimum") &&
                                    path ===
                                        `questionDetailDtos.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide minimmum";
                            return max?.qDetailDesc &&
                                parseInt(value) > parseInt(max?.qDetailDesc)
                                ? "Minimum must not be larger than maximum"
                                : null;
                        }

                        if (
                            values.questionDetailDtos.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("maximum") &&
                                    path ===
                                        `questionDetailDtos.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide maximum";
                            return min?.qDetailDesc &&
                                parseInt(value) < parseInt(min?.qDetailDesc)
                                ? "Maximuam must not be smaller than minimim"
                                : null;
                        }

                        if (
                            values.questionDetailDtos.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("interval") &&
                                    path ===
                                        `questionDetailDtos.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide interval";
                            return answer?.qDetailDesc &&
                                min?.qDetailDesc &&
                                (parseInt(answer.qDetailDesc) -
                                    parseInt(min.qDetailDesc)) %
                                    parseInt(value) !==
                                    0
                                ? "Answer can't be hit with the given interval."
                                : null;
                        }
                    }

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
                detailTypes: (value, values) => {
                    return null;
                },
            },
        },
    });

    const handelSubmit = useCallback(async () => {
        console.log(form.isDirty("qStatement"));
    }, [form.values, fileAudio, fileImage]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row justify-between text-2xl font-bold">
                <h3>Edit Question</h3>
            </div>
            <form
                className="flex flex-col gap-8 relative"
                onSubmit={form.onSubmit(() => {
                    handelSubmit();
                })}
                onReset={() => form.reset()}
            >
                <LoadingOverlay
                    visible={visible}
                    zIndex={1000}
                    overlayProps={{ radius: "sm", blur: 2 }}
                />
                <TextInput
                    label="Statement"
                    variant="filled"
                    withAsterisk
                    classNames={styles}
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
                        classNames={styles}
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
                        classNames={styles}
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
                        classNames={styles}
                        disabled
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
                    classNames={styles}
                />

                <div>
                    <InputLabel>Media</InputLabel>
                    <div className="flex flex-col gap-4 justify-between sm:justify-start ">
                        <label
                            htmlFor="question-image"
                            className="w-[200px] flex gap-4 border text-[#706E6D] bg-[#D9D9D9] px-4 py-3 rounded text-sm cursor-pointer"
                        >
                            <PhotoIcon className="w-5" />
                            <p>Insert Image</p>
                        </label>
                        <div>
                            <div>{fileImage?.name}</div>
                            <div>{humanFileSize(fileImage?.size)}</div>
                        </div>
                        <label
                            htmlFor="question-audio"
                            className="w-[200px] flex gap-4 border text-[#706E6D] bg-[#D9D9D9] px-4 py-3 rounded text-sm cursor-pointer"
                        >
                            <SpeakerWaveIcon className="w-5" />
                            Insert Audio
                        </label>
                        <div>
                            <div>{fileAudio?.name}</div>
                            <div>{humanFileSize(fileAudio?.size)}</div>
                        </div>
                    </div>
                    <FileInput
                        id="question-image"
                        accept="image/png,image/jpeg"
                        className="hidden"
                        value={fileImage}
                        onChange={setFileImage}
                    />
                    <FileInput
                        id="question-audio"
                        className="hidden"
                        accept="audio/*"
                        value={fileAudio}
                        onChange={setFileAudio}
                    />
                </div>

                <QuestionDetailsEdit form={form} />

                <div className="flex justify-end">
                    <Button variant="transparent" color="gray" type="reset">
                        Cancel
                    </Button>
                    <Button
                        variant="filled"
                        color="green"
                        type="submit"
                        disabled={!form.isDirty()}
                    >
                        Update
                    </Button>
                </div>
            </form>
        </div>
    );
}
