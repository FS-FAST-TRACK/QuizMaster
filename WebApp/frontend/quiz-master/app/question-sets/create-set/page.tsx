"use client";

import Pagination from "@/components/Commons/Pagination";
import SearchField from "@/components/Commons/SearchField";
import CreateCategoryModal from "@/components/Commons/modals/CreateCategoryModal";
import CategoriesTable from "@/components/Commons/tables/CategoriesTable";
import {
    CategoryResourceParameter,
    PaginationMetadata,
    Question,
    QuestionCategory,
    QuestionResourceParameter,
    QuestionSet,
    QuestionSetDTO,
} from "@/lib/definitions";
import { fetchCategories, fetchQuestions } from "@/lib/quizData";
import { PlusIcon } from "@heroicons/react/24/outline";
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
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";
import styles from "@/styles/input.module.css";
import { useDisclosure } from "@mantine/hooks";
import QuestionTable from "@/components/Commons/tables/QuestionTable";
import AddQuestionToSetModal from "@/components/Commons/modals/AddQuestionToSetModal";
import { notifications } from "@mantine/notifications";
import notificationStyles from "@/styles/notification.module.css";

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
    const [createSetQuestion, setCreateSetQuestions] = useState(false);
    const [addQuestions, setAddQuestions] = useState(false);
    const [questionSet, setQuestionSet] = useState<Question[]>([]);
    const [qSetName, setQSetName] = useState("");
    const [visible, { toggle, close }] = useDisclosure(false);
    const [paginationMetadata, setPaginationMetadata] = useState<
        PaginationMetadata | undefined
    >();

    const form = useForm<CategoryResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    const handleSubmit = useCallback(async () => {
        const questionSetIds = questionSet.map((question) => question.id);

        const questionSetCreateDto: QuestionSetDTO = {
            qSetDesc: "",
            qSetName: qSetName,
            questions: questionSetIds,
        };
        console.log(questionSetIds);
        console.log(qSetName);
        console.log(JSON.stringify(questionSetCreateDto));
        const res = await fetch(
            `${process.env.QUIZMASTER_GATEWAY}/gateway/api/set/create`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(questionSetCreateDto),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );

        console.log(res);
        if (res.status === 200) {
            notifications.show({
                color: "green",
                title: "Question set created successfully",
                message: "",
                classNames: notificationStyles,
                className: "",
            });
        } else {
            const error = await res.json();
            notifications.show({
                color: "red",
                title: "Failed to create question set",
                message: error.message,
                classNames: notificationStyles,
            });
        }
    }, [questionSet]);

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
                    onChange={(e) => setQSetName(e.target.value)}
                    withAsterisk
                    classNames={styles}
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
                    setSelectedRow={() => null}
                    loading={visible}
                />
                <Pagination form={form} metadata={paginationMetadata} />
                <div className="flex justify-end">
                    <Link
                        className="flex ml-3 h-[40px] items-center gap-3 rounded-md py-3 text-black text-sm font-medium justify-start px-3"
                        href="#"
                    >
                        Cancel
                    </Link>
                    <Button
                        className="flex ml-3 h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                        color="green"
                        onClick={handleSubmit}
                        disabled={qSetName === "" || questionSet.length === 0}
                    >
                        <p className="block">Create a set</p>
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
