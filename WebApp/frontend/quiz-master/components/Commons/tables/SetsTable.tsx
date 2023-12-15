import { QuestionSet, Set } from "@/lib/definitions";
import { fetchAllSetQuestions } from "@/lib/quizData";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import { Checkbox, Loader, Table } from "@mantine/core";
import { useEffect, useState } from "react";
import SetAction from "../popover/SetAction";

export default function QuestionSetsTable({
    questionSets,
    message,
}: {
    questionSets: Set[];
    message?: string;
}) {
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    const [questionsSet, setQuestionsSet] = useState<QuestionSet[]>([]);

    useEffect(() => {
        setSelectedRows([]);

        fetchAllSetQuestions().then((res) => {
            setQuestionsSet(res);
        });
    }, [questionSets]);

    const rows = questionSets.map((questionSet) => {
        const count = questionsSet.filter(
            (question) => question.setId === questionSet.id
        );
        return (
            <Table.Tr
                key={questionSet.id}
                bg={
                    selectedRows.includes(questionSet.id)
                        ? "var(--primary-100)"
                        : undefined
                }
            >
                <Table.Td>
                    <Checkbox
                        color="green"
                        aria-label="Select row"
                        checked={selectedRows.includes(questionSet.id)}
                        onChange={(event) =>
                            setSelectedRows(
                                event.currentTarget.checked
                                    ? [...selectedRows, questionSet.id]
                                    : selectedRows.filter(
                                          (id) => id !== questionSet.id
                                      )
                            )
                        }
                    />
                </Table.Td>
                <Table.Td>{questionSet.qSetName}</Table.Td>
                <Table.Td>{questionSet.dateCreated.toDateString()}</Table.Td>
                <Table.Td>{questionSet.dateUpdated.toDateString()}</Table.Td>
                <Table.Td>{count.length}</Table.Td>
                <Table.Td>
                    <SetAction setId={questionSet.id} onDelete={() => {}} />
                </Table.Td>
            </Table.Tr>
        );
    });

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
                                    selectedRows.length === questionSets.length
                                }
                                onChange={(event) =>
                                    setSelectedRows(
                                        event.currentTarget.checked
                                            ? questionSets.map(
                                                  (questionSet) =>
                                                      questionSet.id
                                              )
                                            : []
                                    )
                                }
                            />
                        </Table.Th>
                        <Table.Th>Question Set</Table.Th>
                        <Table.Th>Created on</Table.Th>
                        <Table.Th>Updated on</Table.Th>
                        <Table.Th>No. of questions</Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {questionSets.length === 0 ? (
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
        </div>
    );
}
