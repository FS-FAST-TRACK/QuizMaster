"use client";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import { useState, useEffect } from "react";
import { cardsData } from "../util/Data";

export default function DragAndDrop() {
  const [data, setData] = useState([]);
  useEffect(() => {
    setData(cardsData);
  }, []);

  const onDragEnd = (e) => {
    const { source, destination } = e;
    if (!destination) return;

    if (source.droppableId === destination.droppableId) {
      const newData = [...data];
      const index = parseInt(source.droppableId.split("droppable")[1]);
      const el = newData[index].components.splice(source.index, 1);
      newData[index].components.splice(destination.index, 0, ...el);
      setData(newData);
    } else {
      const newData = [...data];
      const oldIndex = parseInt(source.droppableId.split("droppable")[1]);
      const newIndex = parseInt(destination.droppableId.split("droppable")[1]);
      const el = newData[oldIndex].components.splice(source.index, 1);
      newData[newIndex].components.splice(destination.index, 0, ...el);
      setData(newData);
    }
  };

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <div className="w-full h-full bg-blue-500 items-center flex flex-col">
        <div>Drag and Drop</div>
        <div className="flex flex-row bg-red-500 space-x-5 w-full">
          {data.length &&
            data.map((data, index) => (
              <Droppable key={data.id} droppableId={`droppable${data.id} test`}>
                {(provided) => (
                  <div
                    key={index}
                    className="flex w-1/2 bg-yellow-500 flex-col items-center"
                    {...provided.droppableProps}
                    ref={provided.innerRef}
                  >
                    <div>{data.title}</div>
                    <div className="space-y-2 w-full">
                      {data.components.map((component, index) => (
                        <Draggable
                          key={component.id}
                          draggableId={`droppable${component.id} test`}
                          index={index}
                        >
                          {(provided) => (
                            <div
                              className="bg-orange-500 w-full flex justify-center"
                              {...provided.dragHandleProps}
                              {...provided.draggableProps}
                              ref={provided.innerRef}
                            >
                              {component.name}
                            </div>
                          )}
                        </Draggable>
                      ))}
                    </div>
                    {provided.placeholder}
                  </div>
                )}
              </Droppable>
            ))}
        </div>
      </div>
    </DragDropContext>
  );
}
