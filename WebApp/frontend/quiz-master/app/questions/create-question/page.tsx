"use client";

import QuestionDetails from "@/components/Commons/QuestionDetails";
import { QuestionCreateValues } from "@/lib/definitions";
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
import {
    Anchor,
    Breadcrumbs,
    Button,
    InputLabel,
    LoadingOverlay,
    Select,
    TextInput,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { useRouter } from "next/navigation";
import { useCallback, useState } from "react";
import styles from "@/styles/input.module.css";
import { notifications } from "@mantine/notifications";
import notificationStyles from "@/styles/notification.module.css";
import ImageInput from "@/components/Commons/inputs/ImageInput";
import AudioInput from "@/components/Commons/inputs/AudioInput";
import { notification } from "@/lib/notifications";
import { postQuestion } from "@/lib/hooks/question";

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
    const [visible, { close, open }] = useDisclosure(false);
    const [fileImage, setFileImage] = useState<File | null>(null);
    const [fileAudio, setFileAudio] = useState<File | null>(null);

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
                    ? "Question statement must not be empty."
                    : null,
            qCategoryId: (value, values) =>
                !value || value?.length === 0
                    ? "Question category is required."
                    : null,
            qDifficultyId: (value, values) =>
                !value || value?.length === 0
                    ? "Question difficulty is required."
                    : null,
            qTypeId: (value, values) =>
                !value || value?.length === 0
                    ? "Question type is required."
                    : null,
            qTime: (value, values) =>
                !value || value?.length === 0
                    ? "Time limit is required."
                    : null,
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

        // Open the loading overlay
        open();

        // Post question
        try {
            const response = await postQuestion({
                question: questionCreateDto,
                image: fileImage,
                audio: fileAudio,
            });

            // Notify for successful post
            if (response.type === "success") {
                notification({
                    type: "success",
                    title: response.message,
                });
                // redirect to qeustions page
                router.push("/questions");
            } else {
                notification({
                    type: "error",
                    title: response.message,
                });
            }
        } catch {
            notification({
                type: "error",
                title: "Something went wrong.",
            });
        }
        close();
    }, [form.values, fileAudio, fileImage]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row justify-between text-2xl font-bold">
                <h3>Create New Question</h3>
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
                        allowDeselect={false}
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
                        allowDeselect={false}
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
                        clearable
                        required
                        allowDeselect={false}
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
                        <ImageInput
                            fileImage={fileImage}
                            setFileImage={setFileImage}
                        />
                        <AudioInput
                            fileAudio={fileAudio}
                            setFileAudio={setFileAudio}
                        />
                    </div>
                </div>

                <QuestionDetails form={form} />

                <div className="flex justify-end">
                    <Button variant="transparent" color="gray" type="reset">
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
