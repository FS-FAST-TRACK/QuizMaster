"use client";

import Pagination from "@/components/Commons/Pagination";
import CreateCategoryModal from "@/components/Commons/modals/CreateCategoryModal";
import PromptModal from "@/components/Commons/modals/PromptModal";
import CategoryAction from "@/components/Commons/popover/CategoryAction";
import CategoriesTable from "@/components/Commons/tables/CategoriesTable";
import {
    CategoryResourceParameter,
    PaginationMetadata,
    QuestionCategory,
    QuestionResourceParameter,
} from "@/lib/definitions";
import { patchCategory, removeCategory } from "@/lib/hooks/category";
import { notification } from "@/lib/notifications";
import { fetchCategories } from "@/lib/quizData";
import { PlusIcon } from "@heroicons/react/24/outline";
import { Anchor, Breadcrumbs, Button, Text } from "@mantine/core";
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
    const [deleteCategory, setDeleteCategory] = useState<
        QuestionCategory | undefined
    >();
    const [editCategory, setEditCategory] = useState<
        QuestionCategory | undefined
    >();

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

    const handleDelete = useCallback(() => {
        if (deleteCategory) {
            removeCategory({ id: deleteCategory?.id })
                .then(() => {
                    setCategories((state) => {
                        var copy = state;
                        const index = copy.findIndex(
                            (qCategory) => qCategory.id === deleteCategory.id
                        );
                        copy.splice(index, 1);
                        return copy;
                    });
                    notification({
                        type: "success",
                        title: `${deleteCategory.qCategoryDesc} category succesfully deleted.`,
                    });
                })
                .catch(() => {
                    notification({
                        type: "error",
                        title: "Failed to delete category.",
                    });
                })
                .finally(() => {
                    setDeleteCategory(undefined);
                });
        }
    }, [deleteCategory]);

    const handleEdit = useCallback(
        async (qCategoryDesc: string) => {
            if (editCategory) {
                patchCategory({
                    id: editCategory?.id,
                    patchRequest: [
                        {
                            path: "qCategoryDesc",
                            op: "replace",
                            value: qCategoryDesc,
                        },
                    ],
                })
                    .then(() => {
                        setCategories((state) => {
                            var copy = state;
                            const index = copy.findIndex(
                                (qCategory) => qCategory.id === editCategory.id
                            );
                            copy[index].qCategoryDesc = qCategoryDesc;

                            return copy;
                        });
                        notification({
                            type: "success",
                            title: "Category succesfully updated.",
                        });
                    })
                    .catch(() => {
                        notification({
                            type: "error",
                            title: "Failed to update category.",
                        });
                    })
                    .finally(() => {
                        setEditCategory(undefined);
                    });
            }
        },
        [editCategory]
    );

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
            <CategoriesTable
                categories={categories}
                onDelete={(cat) => setDeleteCategory(cat)}
                onEdit={(cat) => setEditCategory(cat)}
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
                opened={createCategory || editCategory !== undefined}
                onClose={() => {
                    setCreateCategory(false);
                    setEditCategory(undefined);
                }}
                category={editCategory}
                onUpdate={handleEdit}
            />
            <PromptModal
                body={
                    <div>
                        <Text>Are you sure want to delete.</Text>
                        <div>{deleteCategory?.qCategoryDesc}</div>
                    </div>
                }
                action="Delete"
                onConfirm={handleDelete}
                opened={deleteCategory ? true : false}
                onClose={() => {
                    setDeleteCategory(undefined);
                }}
                title="Delete Category"
            />
        </div>
    );
}
