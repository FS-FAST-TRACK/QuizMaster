"use client";

import Pagination from "@/components/Commons/Pagination";
import SearchField from "@/components/Commons/SearchField";
import QuestionFilter from "@/components/Commons/popover/QuestionFilter";
import QuestionTable from "@/components/Commons/tables/QuestionTable";
import {
    PaginationMetadata,
    Question,
    QuestionResourceParameter,
} from "@/lib/definitions";
import { fetchQuestions } from "@/lib/quizData";
import { PlusIcon } from "@heroicons/react/24/outline";
import { Anchor, Breadcrumbs } from "@mantine/core";
import { useForm } from "@mantine/form";
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
    const [paginationMetadata, setPaginationMetadata] = useState<
        PaginationMetadata | undefined
    >();

    const form = useForm<QuestionResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    useEffect(() => {
        var questionsFetch = fetchQuestions({
            questionResourceParameter: form.values,
        });
        questionsFetch.then((res) => {
            setQuestions(res.data);
            setPaginationMetadata(res.paginationMetadata);
        });
    }, [form.values]);

    const handleSearch = useCallback(() => {
        form.setFieldValue("searchQuery", searchQuery);
    }, [searchQuery, form]);

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

                    <QuestionFilter />
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
            />
            <Pagination form={form} metadata={paginationMetadata} />
        </div>
    );
}
