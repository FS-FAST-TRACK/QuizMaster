"use client";

import {
    PaginationMetadata,
    Question,
    QuestionSetDTO,
    ResourceParameter,
    SetDTO,
} from "@/lib/definitions";
import { PlusIcon } from "@heroicons/react/24/outline";
import {
    Anchor,
    Breadcrumbs,
    Button,
    LoadingOverlay,
    TextInput,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";
import styles from "@/styles/input.module.css";
import { useDisclosure } from "@mantine/hooks";
import QuestionTable from "@/components/Commons/tables/QuestionTable";
import AddQuestionToSetModal from "@/components/Commons/modals/AddQuestionToSetModal";
import { postQuestionSet } from "@/lib/hooks/set";
import { notification } from "@/lib/notifications";
import { useRouter } from "next/navigation";

const items = [
    { label: "All", href: "/question-sets" },
    { label: "Create Set", href: "#" },
    { label: "", href: "#" },
].map((item, index) => (
    <Anchor href={item.href} key={index}>
        <p className="text-black">{item.label}</p>
    </Anchor>
));

export default function Page() {
    const router = useRouter();
    const [addQuestions, setAddQuestions] = useState(false);
    const [questionSet, setQuestionSet] = useState<Question[]>([]);
    const [visible, { close, open }] = useDisclosure(false);
    const [removeQuestion, setRemoveQuestion] = useState<number[]>([]);
    const formValues = useForm<QuestionSetDTO>({
        initialValues: {
            qSetName: "",
            qSetDesc: "",
            questions: [],
            dateCreated: new Date(),
            dateUpdated: new Date(1, 0, 1),
        },
    });

    const form = useForm<ResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    useEffect(() => {
        removeQuestion.map((id) => {
            setQuestionSet((prev) =>
                prev.filter((question) => question.id !== id)
            );
        });
    }, [removeQuestion]);

    const handleSubmit = useCallback(async () => {
        formValues.values.questions = questionSet.map(
            (question) => question.id
        );

        const questionSetCreateDto: SetDTO = {
            qSetDesc: formValues.values.qSetName,
            qSetName: formValues.values.qSetName,
            questions: formValues.values.questions,
        };

        console.log(questionSetCreateDto);
        open();

        postQuestionSet({ questionSet: questionSetCreateDto })
            .then((res) => {
                console.log(res, "hello");
                // Notify for successful post
                notification({
                    type: "success",
                    title: "Question Set created successfuly",
                });
                // redirect to questions set page
                router.push("/question-sets");
            })
            .catch((err) => {
                console.log(err);
                // notify for error
                notification({
                    type: "error",
                    title: "Failed to create Question Set",
                });
            })
            .finally(() => {
                // close loading overlay
                close();
            });
    }, [formValues.values, questionSet]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row justify-between text-2xl font-bold">
                <h3>Create New Question Set</h3>
            </div>
            <form
                className="flex flex-col gap-8 relative"
                onSubmit={form.onSubmit((values) => {
                    console.log(values);
                    //handelSubmit();
                })}
                onReset={() => form.reset()}
            >
                <LoadingOverlay
                    visible={visible}
                    zIndex={1000}
                    overlayProps={{ radius: "sm", blur: 2 }}
                />
                <TextInput
                    label="Set Name"
                    placeholder="Set Name"
                    variant="filled"
                    withAsterisk
                    classNames={styles}
                    {...formValues.getInputProps("qSetName")}
                />

                <div className="flex align-center">
                    <div className="flex flex-col md:flex-row justify-between font-bold">
                        <h4>Added questions</h4>
                    </div>
                    <div className="grow"></div>
                    <Button
                        className="flex h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                        color="green"
                        onClick={() => setAddQuestions(true)}
                    >
                        <PlusIcon className="w-6" />
                        <p className="block">Add Question</p>
                    </Button>
                </div>

                <QuestionTable
                    questions={questionSet}
                    message={
                        form.values.searchQuery
                            ? `No questions match \"${form.values.searchQuery}\"`
                            : questionSet.length === 0
                              ? "No Questions"
                              : undefined
                    }
                    setSelectedRow={setRemoveQuestion}
                    loading={visible}
                    callInQuestionsPage="set"
                />
                <div className="flex justify-end">
                    <Link
                        className="flex ml-3 h-[40px] items-center gap-3 rounded-md py-3 text-black text-sm font-medium justify-start px-3"
                        href="/question-sets"
                    >
                        Cancel
                    </Link>
                    <Button
                        className="flex ml-3 h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                        color="green"
                        onClick={handleSubmit}
                        disabled={
                            formValues.values.qSetName === "" ||
                            questionSet.length === 0
                        }
                    >
                        <p className="block">Create Set</p>
                    </Button>
                </div>
            </form>
            <AddQuestionToSetModal
                onClose={() => setAddQuestions(false)}
                opened={addQuestions}
                setQuestions={setQuestionSet}
                questions={questionSet}
            />
        </div>
    );
}
