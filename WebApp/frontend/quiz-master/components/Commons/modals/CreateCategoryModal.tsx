import { Button, Modal, TextInput } from "@mantine/core";
import { useCallback, useEffect, useState } from "react";
import style from "@/styles/input.module.css";
import { createCategory } from "@/lib/hooks/category";
import { notification } from "@/lib/notifications";
import { QuestionCategory } from "@/lib/definitions";

export type CategoryCreateDto = {
    qCategoryDesc: string;
};
export default function CreateCategoryModal({
    onClose,
    opened,
    onUpdate,
    category,
}: {
    opened: boolean;
    onClose: () => void;
    onUpdate?: (qCategoryDesc: string) => void;
    category?: QuestionCategory;
}) {
    const [categoryDesc, setCategoryDesc] = useState("");
    useEffect(() => {
        if (category) {
            setCategoryDesc(category.qCategoryDesc);
        }
    }, [category]);

    const handelSubmit = useCallback(async () => {
        const categoryCreateDto: CategoryCreateDto = {
            qCategoryDesc: categoryDesc,
        };
        createCategory({ categoryCreateDto })
            .then(() => {
                notification({
                    type: "success",
                    title: "Category created successfully",
                });
                setCategoryDesc("");
            })
            .catch(() => {
                notification({
                    type: "error",
                    title: "Failed to create category",
                });
            })
            .finally(() => {
                onClose();
            });
    }, [category, categoryDesc]);

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            padding={40}
            title={<div className="font-bold">Add Category</div>}
            centered
        >
            <div className="space-y-5">
                <TextInput
                    classNames={style}
                    value={categoryDesc}
                    onChange={(e) => setCategoryDesc(e.target.value)}
                    placeholder="Category"
                />
                <div className="flex justify-end">
                    <Button
                        variant="transparent"
                        color="gray"
                        onClick={onClose}
                    >
                        Cancel
                    </Button>

                    <button
                       
                        className={`${categoryDesc === "" ? "bg-[#5a5a5a20] text-[#5a5a5a] font-medium cursor-not-allowed":"bg-green-600 text-white font-semibold cursor-pointer"}    rounded  p-2`}
                        disabled={categoryDesc === ""}
                        onClick={
                            category && onUpdate
                                ? () => {
                                      onUpdate(categoryDesc);
                                  }
                                : handelSubmit
                        }
                    >
                        {category ? "Update" : "Add Category"}
                    </button>
                </div>
            </div>
        </Modal>
    );
}
