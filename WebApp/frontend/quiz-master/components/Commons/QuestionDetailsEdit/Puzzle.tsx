import { DetailType, QuestionDetail, QuestionValues } from "@/lib/definitions";
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

const getListStyle = (isDraggingOver: boolean) => ({
    background: isDraggingOver ? "var(--primary-100)" : "white",
    padding: 8,
    width: "100%",
    borderRadius: 8,
});

export default function PuzzleQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionValues>;
}) {
    const patchtDetail = (id: number, desc: string) => {
        if (desc !== "") {
            patchQuestionDetail({
                questionId: form.values.id,
                id: id,
                patchRequest: [
                    {
                        path: "/qDetailDesc",
                        op: "replace",
                        value: desc,
                    },
                ],
            });
        }
    };

    const onDragEnd = (result: DropResult) => {
        // dropped outside the list'

        if (!result.destination) {
            return;
        }

        form.reorderListItem("questionDetailDtos", {
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
                            {form.values.questionDetailDtos.map(
                                (item, index) => {
                                    return (
                                        <CustomDraggable
                                            key={index}
                                            form={form}
                                            index={index}
                                            item={item}
                                        />
                                    );
                                }
                            )}
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
                        form.insertListItem("questionDetailDtos", {
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

function CustomDraggable({
    item,
    index,
    form,
}: {
    item: QuestionDetail;
    index: number;
    form: UseFormReturnType<QuestionValues>;
}) {
    const [draggableId, setDraggableId] = useState(`draggable-${index}`);
    useEffect(() => {
        return () => {
            setDraggableId(`draggable-${index}`);
        };
    }, []);
    const patchtDetail = (id: number, desc: string) => {
        if (desc !== "") {
            patchQuestionDetail({
                questionId: form.values.id,
                id: id,
                patchRequest: [
                    {
                        path: "/qDetailDesc",
                        op: "replace",
                        value: desc,
                    },
                ],
            });
        }
    };
    if (!item.detailTypes.includes("answer")) {
        return;
    }
    return (
        <Draggable key={index} draggableId={draggableId} index={index}>
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
                >
                    <Input
                        size="lg"
                        leftSection={<Bars4Icon className="w-6" />}
                        classNames={styles}
                        rightSectionWidth={40}
                        rightSection={
                            <Tooltip label="Remove">
                                <TrashIcon
                                    className="w-6 cursor-pointer"
                                    onClick={() =>
                                        form.removeListItem(
                                            "qDetailDesc",
                                            index
                                        )
                                    }
                                />
                            </Tooltip>
                        }
                        leftSectionPointerEvents="visible"
                        rightSectionPointerEvents="visible"
                        {...form.getInputProps(
                            `questionDetailDtos.${index}.qDetailDesc`
                        )}
                        onBlur={() => {
                            patchtDetail(item.id, item.qDetailDesc);
                        }}
                    />
                </div>
            )}
        </Draggable>
    );
}
