import { Question } from "@/lib/definitions";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import { Box, Checkbox, Loader, Table, Text } from "@mantine/core";
import { useCallback, useEffect, useState } from "react";
import QuesitonAction from "../popover/QuestionAction";
import PromptModal from "../modals/PromptModal";
import QuesitonCard from "../cards/QuestionCard";
import QuestionModal from "../modals/ViewQuestionModal";
import ViewQuestionModal from "../modals/ViewQuestionModal";

export default function QuestionTable({
    questions,
    message,
}: {
    questions: Question[];
    message?: string;
}) {
    const { getQuestionCategoryDescription } = useQuestionCategoriesStore();
    const { getQuestionDifficultyDescription } = useQuestionDifficultiesStore();
    const { getQuestionTypeDescription } = useQuestionTypesStore();

    const [deleteQuestion, setDeleteQuestion] = useState<Question>();
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    const [viewQuestion, setViewQuestion] = useState<Question | undefined>();

    useEffect(() => {
        setSelectedRows([]);
    }, [questions]);

    const handelDelete = useCallback(async () => {
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/${deleteQuestion?.id}`,
            {
                method: "DELETE",
            }
        );
    }, [deleteQuestion]);

    const rows = questions.map((question) => (
        <Table.Tr
            key={question.id}
            bg={
                selectedRows.includes(question.id)
                    ? "var(--primary-100)"
                    : undefined
            }
        >
            <Table.Td>
                <Checkbox
                    color="green"
                    aria-label="Select row"
                    checked={selectedRows.includes(question.id)}
                    onChange={(event) =>
                        setSelectedRows(
                            event.currentTarget.checked
                                ? [...selectedRows, question.id]
                                : selectedRows.filter(
                                      (id) => id !== question.id
                                  )
                        )
                    }
                />
            </Table.Td>
            <Table.Td
                className="cursor-pointer"
                onClick={() => setViewQuestion(question)}
            >
                {question.qStatement}
            </Table.Td>
            <Table.Td>{getQuestionTypeDescription(question.qTypeId)}</Table.Td>
            <Table.Td>
                {getQuestionCategoryDescription(question.qCategoryId)}
            </Table.Td>
            <Table.Td>
                {getQuestionDifficultyDescription(question.qDifficultyId)}
            </Table.Td>
            <Table.Td>
                <QuesitonAction
                    questionId={question.id}
                    onDelete={() => {
                        setDeleteQuestion(question);
                    }}
                />
            </Table.Td>
        </Table.Tr>
    ));

    return (
        <div className="w-full border-2 rounded-xl overflow-x-auto grow bg-white">
            <Table striped>
                <Table.Thead>
                    <Table.Tr>
                        <Table.Th>
                            <Checkbox
                                color="green"
                                aria-label="Select row"
                                checked={
                                    selectedRows.length === questions.length
                                }
                                onChange={(event) =>
                                    setSelectedRows(
                                        event.currentTarget.checked
                                            ? questions.map(
                                                  (question) => question.id
                                              )
                                            : []
                                    )
                                }
                            />
                        </Table.Th>
                        <Table.Th>Question Statement</Table.Th>
                        <Table.Th>Type</Table.Th>
                        <Table.Th>Category</Table.Th>
                        <Table.Th>Difficulty</Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {questions.length === 0 ? (
                        <Table.Tr>
                            <Table.Td colSpan={99} rowSpan={10}>
                                <div className="flex grow justify-center">
                                    {message ? (
                                        message
                                    ) : (
                                        <Loader size={50} color="green" />
                                    )}
                                </div>
                            </Table.Td>
                        </Table.Tr>
                    ) : (
                        rows
                    )}
                </Table.Tbody>
            </Table>
            <PromptModal
                body={
                    <div>
                        <Text>Are you sure want to delete.</Text>
                        <QuesitonCard question={deleteQuestion} />
                    </div>
                }
                action="Delete"
                onConfirm={handelDelete}
                opened={deleteQuestion ? true : false}
                onClose={() => {
                    setDeleteQuestion(undefined);
                }}
                title="Delete Question"
            />

            <ViewQuestionModal
                opened={viewQuestion !== undefined}
                onClose={() => {
                    setViewQuestion(undefined);
                }}
                question={viewQuestion}
            />
        </div>
    );
}
