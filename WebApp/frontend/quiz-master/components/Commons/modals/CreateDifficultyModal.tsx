import { Box, Button, Modal, TextInput } from "@mantine/core";
import { ReactNode, useCallback, useState } from "react";
import style from "@/styles/input.module.css";
import { useRouter } from "next/navigation";

export type DifficultyCreateDto = {
    qDifficultyDesc: string;
};

export default function CreateDifficultyModal({
    onClose,
    opened,
}: {
    opened: boolean;
    onClose: () => void;
}) {
    const router = useRouter();
    const [difficulty, setDifficulty] = useState("");
    const handelSubmit = useCallback(async () => {
        const difficultyCreateDto: DifficultyCreateDto = {
            qDifficultyDesc: difficulty,
        };
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/difficulty`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(difficultyCreateDto),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );

        console.log(res);
        if (res.status === 201) {
            router.push("/difficulties");
            onClose();
        }
    }, [difficulty]);
    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            padding={40}
            title={<div className="font-bold">Add Difficulty</div>}
            centered
        >
            <div className="space-y-5">
                <TextInput
                    classNames={style}
                    value={difficulty}
                    onChange={(e) => setDifficulty(e.target.value)}
                    placeholder="Difficulty"
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
                        disabled={difficulty === ""}
                        onClick={handelSubmit}
                    >
                        Add Difficulty
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
