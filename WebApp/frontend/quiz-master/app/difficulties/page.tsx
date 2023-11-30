"use client";

import Pagination from "@/components/Commons/Pagination";
import SearchField from "@/components/Commons/SearchField";
import CreateDifficultyModal from "@/components/Commons/modals/CreateDifficultyModal";
import DifficultiesTable from "@/components/Commons/tables/DifficultiesTable";
import {
    PaginationMetadata,
    QuestionDifficulty,
    QuestionResourceParameter,
} from "@/lib/definitions";
import { fetchDifficulties } from "@/lib/quizData";
import { PlusIcon } from "@heroicons/react/24/outline";
import { Anchor, Breadcrumbs, Button } from "@mantine/core";
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
    const [createDifficulty, setCreateDifficulty] = useState(false);
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
        var questionsFetch = fetchDifficulties(form.values);
        questionsFetch.then((res) => {
            setDifficulties(res.data);
            setPaginationMetadata(res.paginationMetadata);
        });
    }, [form.values, createDifficulty]);

    const handleSearch = useCallback(() => {
        form.setFieldValue("searchQuery", searchQuery);
    }, [searchQuery, form]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex">
                <Button
                    className="flex h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                    color="green"
                    onClick={() => setCreateDifficulty(true)}
                >
                    <PlusIcon className="w-6" />
                    <p className="block">Create Difficulty</p>
                </Button>
                <div className="grow"></div>

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
            </div>
            <DifficultiesTable
                difficulties={difficulties}
                message={
                    form.values.searchQuery
                        ? `No difficulties match \"${form.values.searchQuery}\"`
                        : difficulties.length === 0
                          ? "No Difficulties"
                          : undefined
                }
            />
            <Pagination form={form} metadata={paginationMetadata} />
            <CreateDifficultyModal
                opened={createDifficulty}
                onClose={() => setCreateDifficulty(false)}
            />
        </div>
    );
}
