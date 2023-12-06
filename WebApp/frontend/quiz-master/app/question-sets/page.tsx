"use client";

import Pagination from "@/components/Commons/Pagination";
import SearchField from "@/components/Commons/SearchField";
import QuestionFilter from "@/components/Commons/popover/QuestionFilter";
import QuestionSetsTable from "@/components/Commons/tables/QuestionSetsTable";
import QuestionTable from "@/components/Commons/tables/QuestionTable";
import {
    PaginationMetadata,
    Question,
    QuestionResourceParameter,
    QuestionSet,
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
    const [questionSets, setQuestionSets] = useState<QuestionSet[]>([]);
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
        //=====START: REMOVE UNCOMMENTED CODES AND UNCOMMENT  COMMENTED CODES IF QUESTION SET API ENDPOINT IS ACCESSIBLE=====
        // var questionsFetch = fetchQuestions({
        //     questionResourceParameter: form.values,
        // });
        // questionsFetch.then((res) => {
        setQuestionSets(mockQuestionSets); //     setQuestionSets(res.data);
        setPaginationMetadata(paginationData); //     setPaginationMetadata(res.paginationMetadata);
        //});
        //=====END======
    }, [form.values]);

    const handleSearch = useCallback(() => {
        form.setFieldValue("searchQuery", searchQuery);
    }, [searchQuery, form]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row gap-3">
                <Link
                    href="question-sets/create-set"
                    className="flex h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                >
                    <PlusIcon className="w-6" />
                    <p className="block">Create a set</p>
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
            <QuestionSetsTable
                questionSets={questionSets}
                message={
                    form.values.searchQuery
                        ? `No question sets match \"${form.values.searchQuery}\"`
                        : questionSets.length === 0
                          ? "No Question Sets"
                          : undefined
                }
            />
            <Pagination form={form} metadata={paginationMetadata} />
        </div>
    );
}

// You can use this mockQuestionSets array in your application DELETE ONCE API ENDPOINT IS ACCESSIBLE

const mockQuestionSets: QuestionSet[] = [
    {
        id: 1,
        setName: "General Knowledge",
        questionCounts: 20,
        dateCreated: new Date("2023-01-01"),
        dateUpdated: new Date("2023-01-05"),
    },
    {
        id: 2,
        setName: "Science Quiz",
        questionCounts: 15,
        dateCreated: new Date("2023-02-10"),
        dateUpdated: new Date("2023-02-15"),
    },
    {
        id: 3,
        setName: "History Trivia",
        questionCounts: 25,
        dateCreated: new Date("2023-03-20"),
        dateUpdated: new Date("2023-03-25"),
    },
    {
        id: 4,
        setName: "Math Challenge",
        questionCounts: 30,
        dateCreated: new Date("2023-04-05"),
        dateUpdated: new Date("2023-04-10"),
    },
    {
        id: 5,
        setName: "Language Puzzles",
        questionCounts: 18,
        dateCreated: new Date("2023-05-15"),
        dateUpdated: new Date("2023-05-20"),
    },
    {
        id: 6,
        setName: "Geography Quiz",
        questionCounts: 22,
        dateCreated: new Date("2023-06-10"),
        dateUpdated: new Date("2023-06-15"),
    },
    {
        id: 7,
        setName: "Art and Music Trivia",
        questionCounts: 25,
        dateCreated: new Date("2023-07-05"),
        dateUpdated: new Date("2023-07-10"),
    },
    {
        id: 8,
        setName: "Sports Challenge",
        questionCounts: 15,
        dateCreated: new Date("2023-08-20"),
        dateUpdated: new Date("2023-08-25"),
    },
    {
        id: 9,
        setName: "Technology Quiz",
        questionCounts: 20,
        dateCreated: new Date("2023-09-10"),
        dateUpdated: new Date("2023-09-15"),
    },
    {
        id: 10,
        setName: "Movie Buff's Delight",
        questionCounts: 24,
        dateCreated: new Date("2023-10-05"),
        dateUpdated: new Date("2023-10-10"),
    },
];

const paginationData: PaginationMetadata = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 8,
    totalPages: 1,
};
