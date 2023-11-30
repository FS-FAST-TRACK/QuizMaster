"use client";

import Pagination from "@/components/Commons/Pagination";
import SearchField from "@/components/Commons/SearchField";
import CreateCategoryModal from "@/components/Commons/modals/CreateCategoryModal";
import CategoriesTable from "@/components/Commons/tables/CategoriesTable";
import {
    CategoryResourceParameter,
    PaginationMetadata,
    QuestionCategory,
    QuestionResourceParameter,
} from "@/lib/definitions";
import { fetchCategories } from "@/lib/quizData";
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
    const [createCategory, setCreateCategory] = useState(false);
    const [categories, setCategories] = useState<QuestionCategory[]>([]);
    const [searchQuery, setSearchQuery] = useState<string>("");
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

    useEffect(() => {
        var categoriesFetch = fetchCategories(form.values);
        categoriesFetch.then((res) => {
            setCategories(res.data);
            setPaginationMetadata(res.paginationMetadata);
        });
    }, [form.values]);

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
                    onClick={() => setCreateCategory(true)}
                >
                    <PlusIcon className="w-6" />
                    <p className="block">Create Category</p>
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
            <CategoriesTable
                categories={categories}
                message={
                    form.values.searchQuery
                        ? `No categories match \"${form.values.searchQuery}\"`
                        : categories.length === 0
                          ? "No Categories"
                          : undefined
                }
            />
            <Pagination form={form} metadata={paginationMetadata} />
            <CreateCategoryModal
                opened={createCategory}
                onClose={() => setCreateCategory(false)}
            />
        </div>
    );
}
