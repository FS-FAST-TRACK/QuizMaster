import { Button, Chip, Modal } from "@mantine/core";
import { useState } from "react";
import { Question } from "@/lib/definitions";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import Link from "next/link";

export default function ViewQuestionModal({
    question,
    onClose,
    opened,
}: {
    question?: Question;
    opened: boolean;
    onClose: () => void;
}) {
    const { getQuestionCategoryDescription } = useQuestionCategoriesStore();
    const { getQuestionDifficultyDescription } = useQuestionDifficultiesStore();
    const { getQuestionTypeDescription } = useQuestionTypesStore();

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            size="lg"
        >
            <div className="space-y-5">
                <div>
                    <div className="flex w-full justify-center">
                        <Chip color="rgba(0, 0, 0, 1)" variant="filled" checked>
                            {question &&
                                getQuestionTypeDescription(question.qTypeId)}
                        </Chip>
                    </div>
                </div>
                <div>
                    <p>Question Statement</p>
                    <p className="text-xl font-bold">{question?.qStatement}</p>
                </div>
                <div className="flex [&>*]:basis-1/3 ">
                    <div>
                        <p>Difficulty</p>
                        <p className="text-xl font-bold">
                            {question &&
                                getQuestionDifficultyDescription(
                                    question?.qDifficultyId
                                )}
                        </p>
                    </div>
                    <div>
                        <p>Category</p>
                        <p className="text-xl font-bold">
                            {question &&
                                getQuestionCategoryDescription(
                                    question?.qCategoryId
                                )}
                        </p>
                    </div>
                    <div>
                        <p>Time Limit</p>
                        <p className="text-xl font-bold">{question?.qTime}</p>
                    </div>
                </div>

                <div className="flex justify-end">
                    <Button
                        variant="transparent"
                        color="gray"
                        onClick={onClose}
                    >
                        Cancel
                    </Button>
                    <Link
                        href={`question/edit-question/${question?.id}`}
                        className="flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-1 text-sm font-medium hover:bg-[--primary-200] justify-start px-3 bg-[--primary] text-white "
                    >
                        Edit Question
                    </Link>
                </div>
            </div>
        </Modal>
    );
}
