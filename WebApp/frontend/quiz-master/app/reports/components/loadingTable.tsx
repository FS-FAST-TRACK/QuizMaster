import React from "react";
import { Table } from "@mantine/core";

export default function LoadingTable({
    columnNames,
    numberOfRows = 20,
}: {
    columnNames: string[];
    numberOfRows: number;
}) {
    const elements = Array(numberOfRows).fill(0);

    const rows = elements.map((_, index) => (
        <Table.Tr key={index} className="h-16">
            {columnNames.map((_, columnIndex) => (
                <Table.Td key={columnIndex}>
                    <div className="bg-gray-200 h-6 rounded-md animate-pulse"></div>
                </Table.Td>
            ))}
        </Table.Tr>
    ));

    return (
        <Table>
            <Table.Thead>
                <Table.Tr>
                    {/* Map through columnNames to create table header cells */}
                    {columnNames.map((columnName, index) => (
                        <Table.Th key={index}>{columnName}</Table.Th>
                    ))}
                </Table.Tr>
            </Table.Thead>
            <Table.Tbody>{rows}</Table.Tbody>
        </Table>
    );
}
