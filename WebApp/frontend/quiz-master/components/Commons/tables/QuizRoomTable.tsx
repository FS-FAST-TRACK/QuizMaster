import { QuizRoom } from "@/lib/definitions/quizRoom";
import { Checkbox, LoadingOverlay, Table, Text } from "@mantine/core";
import { useCallback, useEffect, useState } from "react";

import { notification } from "@/lib/notifications";
import { useQuizRoomsStore } from "@/store/QuizRoomStore";
import PromptModal from "../modals/PromptModal";

export default function QuizRoomTable() {
    const { quizRooms, getPaginatedRooms, pageNumber, pageSize, searchQuery } =
        useQuizRoomsStore();
    const [paginatedRooms, setPaginatedRooms] = useState<QuizRoom[]>([]);
    const [deleteQuizRoom, setDeleteQuizRoom] = useState<QuizRoom>();
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    const [viewQuizRoom, setViewQuizRoom] = useState<QuizRoom | undefined>();

    const handelDelete = useCallback(async () => {
        if (deleteQuizRoom) {
            try {
                // const res = await removeQuizRoom({ id: deleteQuizRoom.id });
                // if (res.type === "success") {
                //     notification({ type: "success", title: res.message });
                // } else {
                //     notification({ type: "error", title: res.message });
                // }
                setDeleteQuizRoom(undefined);
            } catch (error) {
                notification({ type: "error", title: "Something went wrong" });
            }
        }
    }, [deleteQuizRoom]);

    useEffect(() => {
        setPaginatedRooms(
            getPaginatedRooms({ pageNumber, pageSize, searchQuery })
        );
    }, [quizRooms, pageNumber, pageSize, searchQuery]);

    const rows =
        quizRooms &&
        paginatedRooms.map((quizRoom) => (
            <Table.Tr
                key={quizRoom.id}
                bg={
                    selectedRows.includes(quizRoom.id)
                        ? "var(--primary-100)"
                        : undefined
                }
            >
                <Table.Td>
                    <Checkbox
                        color="green"
                        aria-label="Select row"
                        checked={selectedRows.includes(quizRoom.id)}
                        onChange={(event) =>
                            setSelectedRows(
                                event.currentTarget.checked
                                    ? [...selectedRows, quizRoom.id]
                                    : selectedRows.filter(
                                          (id) => id !== quizRoom.id
                                      )
                            )
                        }
                    />
                </Table.Td>

                <Table.Td
                    className="cursor-pointer"
                    onClick={() => setViewQuizRoom(quizRoom)}
                >
                    {quizRoom.qRoomDesc}
                </Table.Td>
                <Table.Td>{quizRoom.dateCreated.toDateString()}</Table.Td>
                <Table.Td>
                    {quizRoom.dateUpdated?.toDateString() || "null"}
                </Table.Td>
                <Table.Td>"Question sets count here"</Table.Td>

                <Table.Td>"Questions count in here"</Table.Td>
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
                                    quizRooms &&
                                    selectedRows.length ===
                                        paginatedRooms.length
                                }
                                onChange={(event) =>
                                    setSelectedRows(
                                        event.currentTarget.checked && quizRooms
                                            ? paginatedRooms.map(
                                                  (quizRoom) => quizRoom.id
                                              )
                                            : []
                                    )
                                }
                            />
                        </Table.Th>
                        <Table.Th>Room Name</Table.Th>
                        <Table.Th>Created on</Table.Th>
                        <Table.Th>Updated on</Table.Th>
                        <Table.Th>Sets</Table.Th>
                        <Table.Th>Questions</Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {quizRooms === undefined ? (
                        <Table.Tr>
                            <Table.Td colSpan={99} rowSpan={10}>
                                <div className="relative h-60">
                                    <LoadingOverlay visible={true} />
                                </div>
                            </Table.Td>
                        </Table.Tr>
                    ) : quizRooms.length === 0 ? (
                        <Table.Tr>
                            <Table.Td colSpan={99} rowSpan={10}>
                                <div className="flex grow justify-center">
                                    No Quiz Rooms found.
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
                        {deleteQuizRoom?.qRoomDesc}
                    </div>
                }
                action="Delete"
                onConfirm={handelDelete}
                opened={deleteQuizRoom ? true : false}
                onClose={() => {
                    setDeleteQuizRoom(undefined);
                }}
                title="Delete QuizRoom"
            />
        </div>
    );
}
