"use client";

import Pagination from "@/components/Commons/Pagination";
import SearchField from "@/components/Commons/SearchField";
import QuestionFilter from "@/components/Commons/popover/QuestionFilter";
import QuestionTable from "@/components/Commons/tables/QuestionTable";
import {
    PaginationMetadata,
    Question,
    QuestionFilterProps,
    ResourceParameter,
} from "@/lib/definitions";
import { fetchQuestions } from "@/lib/hooks/question";
import { notification } from "@/lib/notifications";
import { PlusIcon } from "@heroicons/react/24/outline";
import { Anchor, Breadcrumbs } from "@mantine/core";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";

const items = [
    { label: "All", href: "#" },
    { label: "", href: "#" },
].map((item, index) => (
    <Anchor href={item.href} key={index}>
        <p className="text-black">{item.label}</p>
    </Anchor>
));

export default function Page() {
    const [questions, setQuestions] = useState<Question[]>([]);
    const [searchQuery, setSearchQuery] = useState<string>("");
    const [questionFilters, setQuestionFilters] = useState<QuestionFilterProps>(
        {
            filterByCategories: [],
            filterByDifficulties: [],
            filterByTypes: [],
        }
    );
    const [paginationMetadata, setPaginationMetadata] = useState<
        PaginationMetadata | undefined
    >();

    const [visible, { close, open }] = useDisclosure(true);

    const form = useForm<ResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    const getQuestions = useCallback(async () => {
        try {
            var response = await fetchQuestions({
                questionResourceParameter: {
                    ...form.values,
                    ...questionFilters,
                    exludeQuestionsIds: undefined,
                },
            });
            if (response.type === "success") {
                setQuestions(response.data?.questions!);
                setPaginationMetadata(response.data?.paginationMetada);
            } else {
                notification({
                    type: "error",
                    title: "Failed to fetch questions",
                });
            }
        } catch {
            notification({ type: "error", title: "Something went wrong." });
        }
    }, [form.values, questionFilters]);

    useEffect(() => {
        open();
        getQuestions();
        close();
    }, [form.values, questionFilters]);

    const handleSearch = useCallback(() => {
        form.setFieldValue("searchQuery", searchQuery);
        form.setFieldValue("pageNumber", 1);
    }, [searchQuery, form]);

    const handleFilter = useCallback(
        (filter: QuestionFilterProps) => {
            setQuestionFilters(filter);
            form.setFieldValue("pageNumber", 1);
        },

        [questionFilters, form]
    );

    const onDeleteCallBack = useCallback(() => {
        open();
        getQuestions();
        close();
    }, [form.values, questionFilters]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row gap-3">
                <Link
                    href="questions/create-question"
                    className="flex h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                >
                    <PlusIcon className="w-6" />
                    <p className="block">Create Question</p>
                </Link>
                <div className="grow"></div>

                <div className="flex">
                    <SearchField
                        value={searchQuery}
                        onChange={(e) => {
                            setSearchQuery(e.target.value);
                        }}
                        onKeyDown={(e) => {
                            if (e.code === "Enter") {
                                handleSearch();
                            }
                        }}
                    />

                    <QuestionFilter setQuestionFilters={handleFilter} />
                </div>
            </div>
            <QuestionTable
                questions={questions}
                message={
                    form.values.searchQuery
                        ? `No questions match \"${form.values.searchQuery}\"`
                        : questions.length === 0
                          ? "No Questions"
                          : undefined
                }
                setSelectedRow={() => null}
                loading={visible}
                callInQuestionsPage="questions"
                onDeleteCallBack={onDeleteCallBack}
            />
            <Pagination form={form} metadata={paginationMetadata} />
        </div>
    );
}
