"use client";

import QuestionDetailsEdit from "@/components/Commons/QuestionDetailsEdit";
import {
    Question,
    QuestionEdit,
    QuestionValues,
    PatchItem,
} from "@/lib/definitions";
import { GetPatches, humanFileSize } from "@/lib/helpers";
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
import ImageInput from "@/components/Commons/inputs/ImageInput";
import AudioInput from "@/components/Commons/inputs/AudioInput";
import { fetchQuestion, patchQuestion } from "@/lib/hooks/question";
import { useErrorRedirection } from "@/utils/errorRedirection";
import { notification } from "@/lib/notifications";

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
    const { redirectToError } = useErrorRedirection();
    const { questionDifficulties } = useQuestionDifficultiesStore();
    const { questionTypes } = useQuestionTypesStore();
    const router = useRouter();
    const [visible, { toggle, close }] = useDisclosure(false);
    const [fileImage, setFileImage] = useState<File | null>(null);
    const [fileAudio, setFileAudio] = useState<File | null>(null);
    const [isFetching, setIsFetching] = useState<boolean>(false);

    const populateQuestion = useCallback(async () => {
        try {
            const response = await fetchQuestion({ questionId: params.id });

            if (response.type === "success") {
                const question = response.data!.question;
                form.setInitialValues({
                    id: question.id,
                    qStatement: question.qStatement,
                    qAudio: question.qAudio,
                    qImage: question.qImage,
                    qCategoryId: question.qCategoryId.toString(),
                    qDifficultyId: question.qDifficultyId.toString(),
                    qTypeId: question.qTypeId.toString(),
                    qTime: question.qTime.toString(),
                    details: question.details,
                });

                form.setValues({
                    id: question.id,
                    qStatement: question.qStatement,
                    qAudio: question.qAudio,
                    qImage: question.qImage,
                    qCategoryId: question.qCategoryId.toString(),
                    qDifficultyId: question.qDifficultyId.toString(),
                    qTypeId: question.qTypeId.toString(),
                    qTime: question.qTime.toString(),
                    details: question.details,
                });
            }
        } catch (error) {
            notification({ type: "error", title: "Something went wrong." });
            redirectToError();
        }
    }, [params]);

    useEffect(() => {
        populateQuestion();
    }, []);

    const form = useForm<
        QuestionEdit,
        (questionEdit: QuestionEdit) => PatchItem[]
    >({
        initialValues: {
            id: 1,
            qStatement: "",
            qAudio: "",
            qImage: "",
            qCategoryId: "1",
            qDifficultyId: "1",
            qTypeId: "1",
            qTime: "30",
            details: [],
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        transformValues: (values) => {
            var patches: PatchItem[] = [];
            if (form.isDirty("qStatement")) {
                patches = [
                    {
                        path: "qStatement",
                        op: "replace",
                        value: form.values.qStatement,
                    },
                    ...patches,
                ];
            }
            if (form.isDirty("qCategoryId")) {
                patches = [
                    {
                        path: "qCategoryId",
                        op: "replace",
                        value: parseInt(form.values.qCategoryId),
                    },
                    ...patches,
                ];
            }
            if (form.isDirty("qDifficultyId")) {
                patches = [
                    {
                        path: "qDifficultyId",
                        op: "replace",
                        value: parseInt(form.values.qDifficultyId),
                    },
                    ...patches,
                ];
            }
            if (form.isDirty("qTime")) {
                patches = [
                    {
                        path: "qTime",
                        op: "replace",
                        value: parseInt(form.values.qTime),
                    },
                    ...patches,
                ];
            }
            return patches;
        },
        validate: {
            qStatement: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
            qCategoryId: (value, values) =>
                value?.length === 0 ? "Question Category is required." : null,
        },
    });

    const updateQuestion = useCallback(async () => {
        const patches = form.getTransformedValues();
        try {
            const response = await patchQuestion({
                id: params.id,
                patches: patches,
                image: fileImage,
                audio: fileAudio,
            });
            if (response.type === "success") {
                notification({
                    type: "success",
                    title: "Successfuly updated.",
                });
                form.setInitialValues({
                    ...form.values,
                    qAudio: response.data!.qAudio || "",
                    qImage: response.data!.qImage || "",
                });

                form.setValues({
                    ...form.values,
                    qAudio: response.data!.qAudio || "",
                    qImage: response.data!.qImage || "",
                });
                setFileAudio(null);
                setFileImage(null);
                form.resetDirty();
            } else {
                notification({
                    type: "error",
                    title: response.message,
                });
            }
        } catch (error) {
            notification({
                type: "error",
                title: "Something went wrong.",
                message: "Unable to update question",
            });
        }
    }, [form.values, fileAudio, fileImage]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row justify-between text-2xl font-bold">
                <h3>Edit Question</h3>
            </div>
            <form
                className="flex flex-col gap-8 relative"
                onReset={() => {
                    form.reset();
                    setFileAudio(null), setFileImage(null);
                }}
                onSubmit={(e) => {
                    e.preventDefault();
                    updateQuestion();
                }}
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
                        <ImageInput
                            fileImage={fileImage}
                            setFileImage={setFileImage}
                            qImageId={
                                form.values.qImage.length > 15
                                    ? form.values.qImage
                                    : undefined
                            }
                        />
                        <AudioInput
                            fileAudio={fileAudio}
                            setFileAudio={setFileAudio}
                            qAudioId={
                                form.values.qAudio.length > 15
                                    ? form.values.qAudio
                                    : undefined
                            }
                        />
                    </div>
                </div>
                <div className="flex justify-end gap-3">
                    <Button
                        variant="transparent"
                        color="gray"
                        disabled={
                            !(
                                form.isDirty() ||
                                fileAudio !== null ||
                                fileImage !== null
                            )
                        }
                        type="reset"
                    >
                        Cancel
                    </Button>
                    <Button
                        variant="filled"
                        color="green"
                        disabled={
                            !(
                                form.isDirty() ||
                                fileAudio !== null ||
                                fileImage !== null
                            )
                        }
                        type="submit"
                    >
                        Update
                    </Button>
                </div>
            </form>
            <QuestionDetailsEdit
                questionId={form.values.id}
                questionTypeId={form.values.qTypeId}
                details={form.values.details}
            />
        </div>
    );
}
