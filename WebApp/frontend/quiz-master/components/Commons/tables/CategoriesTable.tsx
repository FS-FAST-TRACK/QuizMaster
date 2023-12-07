import { QuestionCategory } from "@/lib/definitions";
import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import { Checkbox, Loader, Table } from "@mantine/core";
import { ReactNode, useEffect, useState } from "react";
import ViewCategoryModal from "../modals/ViewCategoryModal";
import CategoryAction from "../popover/CategoryAction";

export default function CategoriesTable({
    categories,
    message,
    onEdit,
    onDelete,
}: {
    categories: QuestionCategory[];
    message?: string;
    onEdit?: (category: QuestionCategory) => void;
    onDelete?: (category: QuestionCategory) => void;
}) {
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    const [viewCategory, setViewCategory] = useState<
        QuestionCategory | undefined
    >();

    useEffect(() => {
        setSelectedRows([]);
    }, [categories]);

    const rows = categories.map((category) => (
        <Table.Tr
            key={category.id}
            bg={
                selectedRows.includes(category.id)
                    ? "var(--primary-100)"
                    : undefined
            }
        >
            <Table.Td>
                <Checkbox
                    color="green"
                    aria-label="Select row"
                    checked={selectedRows.includes(category.id)}
                    onChange={(event) =>
                        setSelectedRows(
                            event.currentTarget.checked
                                ? [...selectedRows, category.id]
                                : selectedRows.filter(
                                      (id) => id !== category.id
                                  )
                        )
                    }
                />
            </Table.Td>
            <Table.Td
                className="cursor-pointer"
                onClick={() => setViewCategory(category)}
            >
                {category.qCategoryDesc}
            </Table.Td>
            <Table.Td>{category.dateCreated.toDateString()}</Table.Td>
            <Table.Td>{category.dateUpdated.toDateString()}</Table.Td>
            <Table.Td>{category.questionCounts}</Table.Td>
            {onDelete && onEdit && (
                <Table.Td>
                    <CategoryAction
                        onDelete={() => onDelete(category)}
                        onEdit={() => onEdit(category)}
                    />
                </Table.Td>
            )}
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
                                    selectedRows.length === categories.length
                                }
                                onChange={(event) =>
                                    setSelectedRows(
                                        event.currentTarget.checked
                                            ? categories.map(
                                                  (category) => category.id
                                              )
                                            : []
                                    )
                                }
                            />
                        </Table.Th>
                        <Table.Th>Category</Table.Th>
                        <Table.Th>Created on</Table.Th>
                        <Table.Th>Updated on</Table.Th>
                        <Table.Th>Questions</Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {categories.length === 0 ? (
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
            <ViewCategoryModal
                opened={viewCategory !== undefined}
                onClose={() => {
                    setViewCategory(undefined);
                }}
                category={viewCategory}
            />
        </div>
    );
}
