import {
    DetailType,
    QuestionDetail,
    QuestionEdit,
    QuestionValues,
} from "@/lib/definitions";
import {
    Bars4Icon,
    PlusCircleIcon,
    TrashIcon,
} from "@heroicons/react/24/outline";
import { Button, Input, InputLabel, Tooltip } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import { useEffect, useState } from "react";
import {
    DragDropContext,
    Draggable,
    DropResult,
    Droppable,
} from "react-beautiful-dnd";
import styles from "@/styles/input.module.css";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import PuzzleInput from "../inputs/PuzzleInput";

const getListStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "var(--primary-100)" : "white",
    padding: 8,
    width: "100%",
    borderRadius: 8,
});

export default function PuzzleQuestionDetails({
    form,
}: {
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    const onDragEnd = (result: DropResult) => {
        // dropped outside the list'

        if (!result.destination) {
            return;
        }

        form.reorderListItem("details", {
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
                            {form.values.details.map((item, index) => {
                                return (
                                    <CustomDraggable
                                        key={index}
                                        form={form}
                                        index={index}
                                        item={item}
                                    />
                                );
                            })}
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
                        form.insertListItem("details", {
                            qDetailDesc: "",
                            detailTypes: ["option"],
                        })
                    }
                >
                    <PlusCircleIcon className="w-6" />
                </Button>
            </DragDropContext>
        </div>
    );
}

function CustomDraggable({
    item,
    index,
    form,
}: {
    item: QuestionDetail;
    index: number;
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    const [draggableId, setDraggableId] = useState(`draggable-${index}`);
    useEffect(() => {
        return () => {
            setDraggableId(`draggable-${index}`);
        };
    }, []);
    // const patchtDetail = (id: number, desc: string) => {
    //     if (desc !== "") {
    //         patchQuestionDetail({
    //             questionId: form.values.id,
    //             id: id,
    //             patchRequest: [
    //                 {
    //                     path: "/qDetailDesc",
    //                     op: "replace",
    //                     value: desc,
    //                 },
    //             ],
    //         });
    //     }
    // };
    if (!item.detailTypes.includes("option")) {
        return;
    }
    return (
        <Draggable
            key={index}
            draggableId={`draggable1-${index}`}
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
                        ...provided.draggableProps.style,
                    }}
                    className="relative"
                >
                    <PuzzleInput
                        onRemove={() => {
                            form.removeListItem("details", index);
                        }}
                        {...form.getInputProps(`details.${index}.qDetailDesc`)}
                    />
                </div>
            )}
        </Draggable>
    );
}
