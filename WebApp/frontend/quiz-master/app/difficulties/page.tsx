"use client";

import Pagination from "@/components/Commons/Pagination";
import DifficultiesTable from "@/components/Commons/tables/DifficultiesTable";
import {
    PaginationMetadata,
    QuestionDifficulty,
    QuestionResourceParameter,
} from "@/lib/definitions";
import { fetchDifficulties } from "@/lib/quizData";
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
    const [difficulties, setDifficulties] = useState<QuestionDifficulty[]>([]);
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
        var questionsFetch = fetchDifficulties();
        questionsFetch.then((res) => {
            setDifficulties(res);
        });
    }, [form.values]);

    const handleSearch = useCallback(() => {
        form.setFieldValue("searchQuery", searchQuery);
    }, [searchQuery, form]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex">
                <Link
                    href="questions/create-question"
                    className="flex h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                >
                    <PlusIcon className="w-6" />
                    <p className="block">Create Difficulty</p>
                </Link>
                <div className="grow"></div>

                <input
                    className="h-[40px] rounded-lg px-5 focus:outline-green-500"
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
            </div>
            <DifficultiesTable difficulties={difficulties} />
            <Pagination form={form} metadata={paginationMetadata} />
        </div>
    );
}
