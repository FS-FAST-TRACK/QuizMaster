import { Box, Button, Modal, TextInput } from "@mantine/core";
import { ReactNode, useCallback, useEffect, useState } from "react";
import style from "@/styles/input.module.css";
import { useRouter } from "next/navigation";
import { notifications } from "@mantine/notifications";
import notificationStyles from "../../../styles/notification.module.css";
import { QuestionDifficulty } from "@/lib/definitions";
import { createDifficulty } from "@/lib/hooks/difficulty";
import { notification } from "@/lib/notifications";

export type DifficultyCreateDto = {
    qDifficultyDesc: string;
};

export default function CreateDifficultyModal({
    onClose,
    opened,
    onUpdate,
    difficulty,
}: {
    opened: boolean;
    onClose: () => void;
    onUpdate?: (qCategoryDesc: string) => void;
    difficulty?: QuestionDifficulty;
}) {
    const [difficultyDesc, setDifficultyDesc] = useState("");

    useEffect(() => {
        if (difficulty) {
            setDifficultyDesc(difficulty.qDifficultyDesc);
        }
    }, [difficulty]);

    const handelSubmit = useCallback(async () => {
        const difficultyCreateDto: DifficultyCreateDto = {
            qDifficultyDesc: difficultyDesc,
        };
        createDifficulty({ difficultyCreateDto })
            .then(() => {
                notification({
                    type: "success",
                    title: "Difficulty created successfully",
                });
                setDifficultyDesc("");
            })
            .catch(() => {
                notification({
                    type: "error",
                    title: "Failed to create difficulty",
                });
            })
            .finally(() => {
                onClose();
            });
    }, [difficulty, difficultyDesc]);

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
                    value={difficultyDesc}
                    onChange={(e) => setDifficultyDesc(e.target.value)}
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
                        disabled={difficultyDesc === ""}
                        onClick={
                            difficulty && onUpdate
                                ? () => {
                                      onUpdate(difficultyDesc);
                                  }
                                : handelSubmit
                        }
                    >
                        {difficulty ? "Update" : "Add Difficulty"}
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
