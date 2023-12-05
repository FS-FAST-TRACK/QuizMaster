import { QuestionCreateValues } from "@/lib/definitions";
import {
    Bars4Icon,
    PlusCircleIcon,
    TrashIcon,
} from "@heroicons/react/24/outline";
import { Button, Input, InputLabel, Text, Tooltip } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import React, { useEffect, useState } from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import styles from "@/styles/input.module.css";

const getListStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "var(--primary-100)" : "white",
    padding: 8,
    width: "100%",
    borderRadius: 8,
});

export default function PuzzleQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
    const onDragEnd = (result: any) => {
        // dropped outside the list'

        if (!result.destination) {
            return;
        }

        form.reorderListItem("options", {
            from: result.source.index,
            to: result.destination.index,
        });
    };
    const [droppableID, setDroppableId] = useState("droppable");
    useEffect(() => {
        return () => {
            setDroppableId("droppableID");
        };
    }, []);

    return (
        <div className="flex flex-col max-w-96">
            <InputLabel>Choices</InputLabel>

            <DragDropContext onDragEnd={(result) => onDragEnd(result)}>
                <Droppable droppableId={droppableID}>
                    {(provided, snapshot) => (
                        <div
                            {...provided.droppableProps}
                            ref={provided.innerRef}
                            style={getListStyle(snapshot.isDraggingOver)}
                        >
                            {form.values.options.map((option, index) => (
                                <Draggable
                                    key={index}
                                    draggableId={index + "id"}
                                    index={index}
                                >
                                    {(provided, snapshot) => (
                                        <div
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                            style={{
                                                userSelect: "none",
                                                margin: `0 0 8px 0`,
                                                ...provided.draggableProps
                                                    .style,
                                            }}
                                        >
                                            <Input
                                                size="lg"
                                                leftSection={
                                                    <Bars4Icon className="w-6" />
                                                }
                                                classNames={styles}
                                                rightSectionWidth={40}
                                                rightSection={
                                                    <Tooltip label="Remove">
                                                        <TrashIcon
                                                            className="w-6 cursor-pointer"
                                                            onClick={() =>
                                                                form.removeListItem(
                                                                    "options",
                                                                    index
                                                                )
                                                            }
                                                        />
                                                    </Tooltip>
                                                }
                                                leftSectionPointerEvents="visible"
                                                rightSectionPointerEvents="visible"
                                                {...form.getInputProps(
                                                    `options.${index}.value`
                                                )}
                                            />
                                        </div>
                                    )}
                                </Draggable>
                            ))}
                            {provided.placeholder}
                        </div>
                    )}
                </Droppable>
                <Button
                    variant="outline"
                    color="gray"
                    size="lg"
                    className="border-4 outline-2 outline-gray-800 w-full"
                    onClick={() =>
                        form.insertListItem("options", {
                            value: "",
                            isAnswer: false,
                        })
                    }
                >
                    <PlusCircleIcon className="w-6" />
                </Button>
            </DragDropContext>
        </div>
    );
}
