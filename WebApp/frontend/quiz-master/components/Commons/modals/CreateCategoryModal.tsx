import { Box, Button, Modal, TextInput } from "@mantine/core";
import { ReactNode, useCallback, useState } from "react";
import style from "@/styles/input.module.css";
import { useRouter } from "next/navigation";

export type CategoryCreateDto = {
    qCategoryDesc: string;
};
export default function CreateCategoryModal({
    onClose,
    opened,
}: {
    opened: boolean;
    onClose: () => void;
}) {
    const router = useRouter();
    const [category, setCategory] = useState("");
    const handelSubmit = useCallback(async () => {
        const categoryCreateDto: CategoryCreateDto = {
            qCategoryDesc: category,
        };
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(categoryCreateDto),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );

        console.log(res);
        if (res.status === 201) {
            router.push("/categories");
        }
    }, [category]);
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
                    value={category}
                    onChange={(e) => setCategory(e.target.value)}
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
                    <Button
                        variant="filled"
                        color="green"
                        disabled={category === ""}
                        onClick={handelSubmit}
                    >
                        Add Category
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
