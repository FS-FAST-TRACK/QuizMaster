"use client";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import { useState, useEffect } from "react";
import { cardsData } from "../util/Data";
import { Button } from "@mantine/core";
import drag from "@/public/icons/drag.png";
import Image from "next/image";

export default function DragAndDrop() {
  const [data, setData] = useState([]);
  const [answer, setAnswer] = useState([]);
  const [droppableId, setDroppableId] = useState("droppable");

  const maxNewDataItems = 4;
  useEffect(() => {
    setData(cardsData);
    setDroppableId("droppableId");
  }, []);

  const onDragEnd = (result) => {
    if (!result.destination) return;

    const sourceIndex = result.source.index;
    const destinationIndex = result.destination.index;
    console.log(answer.length);
    if (result.source.droppableId === result.destination.droppableId) {
      // Dragged within the same droppable

      if (result.source.droppableId === `${droppableId} answer`) {
        const updatedData = [...answer];
        const [movedItem] = updatedData.splice(sourceIndex, 1);
        updatedData.splice(destinationIndex, 0, movedItem);
        setAnswer(() => updatedData);
      } else {
        const updatedData = [...data];
        const [movedItem] = updatedData.splice(sourceIndex, 1);
        updatedData.splice(destinationIndex, 0, movedItem);
        setData(updatedData);
      }
    } else {
      // Dragged to a different droppable
      if (result.source.droppableId === `${droppableId} answer`) {
        const updatedData = [...answer];
        const [movedItem] = updatedData.splice(sourceIndex, 1);
        setAnswer(updatedData);

        const updatedNewData = [...data];
        updatedNewData.splice(destinationIndex, 0, movedItem);
        setData(updatedNewData);
      } else {
        if (answer.length + 1 > maxNewDataItems) {
          console.log("lapas");
          return;
        }
        const updatedData = [...data];
        const [movedItem] = updatedData.splice(sourceIndex, 1);
        setData(updatedData);

        const updatedNewData = [...answer];
        updatedNewData.splice(destinationIndex, 0, movedItem);
        setAnswer(() => updatedNewData);
      }
    }

    // if (source.droppableId === destination.droppableId) {
    //   const newData = [...data];
    //   const el = newData.splice(source.index, 1);
    //   newData.splice(destination.index, 0, ...el);
    //   setData(newData);
    // } else {
    //   const newData = [...data];
    //   const oldIndex = parseInt(source.droppableId.split("droppable")[1]);
    //   const newIndex = parseInt(destination.droppableId.split("droppable")[1]);
    //   const el = newData[oldIndex].components.splice(source.index, 1);
    //   newData[newIndex].components.splice(destination.index, 0, ...el);
    //   setData(newData);
    // }
  };

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <div className="w-full h-full  items-center flex flex-col">
        <div className="flex flex-col w-full h-full">
          <div className="flex-grow items-center justify-center flex flex-col">
            <div className="text-white">Puzzle</div>
            <div className="font-bold text-xl text-white">
              Arrange the following steps in the correct order to perform a
              bubble sort on an array
            </div>
            <div className="w-full">
              <Droppable droppableId={`${droppableId} answer`}>
                {(provided) => (
                  <div
                    className="flex w-full flex-col items-center"
                    {...provided.droppableProps}
                    ref={provided.innerRef}
                  >
                    <div className=" w-full items-center flex flex-col ">
                      {answer?.map((component, index) => (
                        <Draggable
                          key={component.id}
                          draggableId={`droppable${component.id}`}
                          index={index}
                        >
                          {(provided) => (
                            <div
                              className="bg-white flex w-1/2    items-center text-xl font-bold p-1 m-2 text-green_text shadow-lg rounded-lg px-2"
                              {...provided.dragHandleProps}
                              {...provided.draggableProps}
                              ref={provided.innerRef}
                            >
                              <div>
                                <Image src={drag} className="" />
                              </div>
                              <div className="flex-grow  justify-center  flex">
                                {component?.option}
                              </div>
                            </div>
                          )}
                        </Draggable>
                      ))}
                    </div>
                    {Array.from({
                      length: Math.max(0, maxNewDataItems - answer.length),
                    }).map((_, index) => (
                      <div
                        key={`placeholder-${index}`}
                        className="bg-dark_green flex w-1/2  justify-center m-2 h-10"
                      ></div>
                    ))}
                    {provided.placeholder}
                  </div>
                )}
              </Droppable>
            </div>
          </div>

          <div className="w-f flex items-center flex-row h-fit flex-wrap p-2 ">
            <Droppable droppableId={`${droppableId} option`}>
              {(provided) => (
                <div
                  className="flex w-full  flex-col items-center "
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                >
                  <div className="w-full grid grid-cols-2 place-content-center  ">
                    {data?.map((component, index) => (
                      <Draggable
                        key={component?.id}
                        draggableId={`droppable${component?.id}`}
                        index={index}
                      >
                        {(provided) => (
                          <div
                            className="bg-white flex    items-center text-xl font-bold p-1 m-2 text-green_text shadow-lg rounded-lg px-2"
                            {...provided.dragHandleProps}
                            {...provided.draggableProps}
                            ref={provided.innerRef}
                          >
                            <div>
                              <Image src={drag} className="" />
                            </div>
                            <div className="flex-grow  justify-center  flex">
                              {component?.option}
                            </div>
                          </div>
                        )}
                      </Draggable>
                    ))}
                  </div>

                  {provided.placeholder}
                </div>
              )}
            </Droppable>
          </div>
          <div className=" w-full justify-center flex">
            <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
              <Button fullWidth color={"yellow"}>
                Sumbit
              </Button>
            </div>
          </div>
        </div>
      </div>
    </DragDropContext>
  );
}
