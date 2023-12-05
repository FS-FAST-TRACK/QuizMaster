import { QuestionDifficulty } from "@/lib/definitions";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import { Checkbox, Loader, Table } from "@mantine/core";
import { useEffect, useState } from "react";

export default function DifficultiesTable({
    difficulties,
    message,
}: {
    difficulties: QuestionDifficulty[];
    message?: string;
}) {
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    useEffect(() => {
        setSelectedRows([]);
    }, [difficulties]);

    const rows = difficulties.map((difficulty) => (
        <Table.Tr
            key={difficulty.id}
            bg={
                selectedRows.includes(difficulty.id)
                    ? "var(--primary-100)"
                    : undefined
            }
        >
            <Table.Td>
                <Checkbox
                    color="green"
                    aria-label="Select row"
                    checked={selectedRows.includes(difficulty.id)}
                    onChange={(event) =>
                        setSelectedRows(
                            event.currentTarget.checked
                                ? [...selectedRows, difficulty.id]
                                : selectedRows.filter(
                                      (id) => id !== difficulty.id
                                  )
                        )
                    }
                />
            </Table.Td>
            <Table.Td>{difficulty.qDifficultyDesc}</Table.Td>
            <Table.Td>{difficulty.dateCreated.toDateString()}</Table.Td>
            <Table.Td>{difficulty.dateUpdated.toDateString()}</Table.Td>
            <Table.Td>{difficulty.questionCounts}</Table.Td>
            <Table.Td>
                <div className="cursor-pointer flex items-center justify-center aspect-square">
                    <EllipsisVerticalIcon className="w-6" />
                </div>
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
                                    selectedRows.length === difficulties.length
                                }
                                onChange={(event) =>
                                    setSelectedRows(
                                        event.currentTarget.checked
                                            ? difficulties.map(
                                                  (difficulty) => difficulty.id
                                              )
                                            : []
                                    )
                                }
                            />
                        </Table.Th>
                        <Table.Th>Difficulty</Table.Th>
                        <Table.Th>Created on</Table.Th>
                        <Table.Th>Updated on</Table.Th>
                        <Table.Th>Questions</Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {difficulties.length === 0 ? (
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
