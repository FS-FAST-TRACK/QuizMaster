"use client";
import { DragDropContext, Droppable, Draggable } from "@hello-pangea/dnd";
import React, { useState, useEffect } from "react";
import { Button } from "@mantine/core";
import drag from "@/public/icons/drag.png";
import Image from "next/image";
import { submitAnswer, uploadScreenshot } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";
import { useScreenshot } from "use-react-screenshot";

export default React.forwardRef(DragAndDrop);

function DragAndDrop({ question, connectionId }, ref) {
  const [data, setData] = useState([]);
  const [answer, setAnswer] = useState([]);
  const [droppableId, setDroppableId] = useState("droppable");
  const [answerDetail, setAnswerDetail] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [hasImage, setHasImage] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [previousStatement, setPreviousStatement] = useState(null);
  const [opened, { open, close }] = useDisclosure(false);
  const [image, takeScreenShot] = useScreenshot({
    type: "image/jpeg",
    quality: 1.0,
  });

  useEffect(() => {
    if (question?.question.qImage) {
      downloadImage({
        url: question.question.qImage,
        setImageUrl: setImageUrl,
        setHasImage: setHasImage,
      });
      setHasImage(true);
    }
  }, [question?.question.qImage, previousStatement]);

  useEffect(() => {
    if (question?.question.qStatement !== previousStatement) {
      const options = question?.details;

      // Copy all elements except the last one
      const copiedData = [...options?.slice(0, options?.length - 1)];
      //Get the number of possible answers
      const lastElement = options[options.length - 1];
      const detail = JSON.parse(lastElement.qDetailDesc);
      setAnswerDetail(detail.length);

      setData(copiedData);
      setDroppableId("droppableId");
      setAnswer([]);
      setIsSubmitted(false);
      setHasImage(false);
      setImageUrl();
      setPreviousStatement(question?.question.qStatement);
    }
  }, [question?.question.qStatement]);

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
        if (answer.length + 1 > answerDetail) {
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
  };

  const submitScreenshot = (id, connectionId) => takeScreenShot(ref.current).then((image) => uploadScreenshot(image, id, connectionId));

  const handleSubmit = () => {
    console.log("On Submit");
    console.log(answer);
    const arrayOfIds = answer?.map((opt) => opt.id);
    console.log(arrayOfIds);
    const idsString = JSON.stringify(arrayOfIds);

    console.log(idsString);
    let id = question.question.id;
    setIsSubmitted(true);
    submitScreenshot(id, connectionId);
    submitAnswer({ id, answer: idsString, connectionId });
  };

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="w-full h-full  items-center flex flex-col p-4">
        <div className="flex flex-col w-full h-full">
          <div className="flex-grow items-center justify-center flex flex-col">
            <div className="text-white">Puzzle</div>
            {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
            <div className="font-bold text-xl text-white">
              {question?.question.qStatement}
            </div>
            <div className="w-full flex justify-center">
              <Droppable
                droppableId={`${droppableId} answer`}
                isDropDisabled={isSubmitted}
              >
                {(provided) => (
                  <div
                    className="flex w-1/2 flex-col items-center "
                    {...provided.droppableProps}
                    ref={provided.innerRef}
                  >
                    <div className=" w-full  items-center flex flex-col  ">
                      {answer?.map((option, index) => (
                        <Draggable
                          key={option.id}
                          draggableId={`droppable${option.id}`}
                          index={index}
                        >
                          {(provided) => (
                            <div
                              className="bg-white flex w-full    items-center text-xl font-bold p-1 m-2 text-green_text shadow-lg rounded-lg px-2"
                              {...provided.dragHandleProps}
                              {...provided.draggableProps}
                              ref={provided.innerRef}
                            >
                              <div>
                                <Image src={drag} className="" />
                              </div>
                              <div className="flex-grow  justify-center  flex">
                                {option?.qDetailDesc}
                              </div>
                            </div>
                          )}
                        </Draggable>
                      ))}
                    </div>
                    {Array.from({
                      length: Math.max(0, answerDetail - answer.length),
                    }).map((_, index) => (
                      <div
                        key={`placeholder-${index}`}
                        className="bg-dark_green flex w-full  justify-center m-2 h-10 rounded-lg"
                      ></div>
                    ))}
                  </div>
                )}
              </Droppable>
            </div>
          </div>

          <div className="w-full flex items-center flex-col h-1/4 flex-wrap p-2  ">
            <Droppable
              droppableId={`${droppableId} option`}
              direction="vertical"
              isDropDisabled={isSubmitted}
            >
              {(provided) => (
                <div
                  className="flex w-full flex-row items-center  "
                  {...provided.droppableProps}
                  ref={provided.innerRef}
                >
                  <div className="w-full grid grid-cols-2 place-content-center  ">
                    {data?.map((option, index) => (
                      <Draggable
                        key={option?.id}
                        draggableId={`droppable${option?.id}`}
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
                              {option?.qDetailDesc}
                            </div>
                          </div>
                        )}
                      </Draggable>
                    ))}
                  </div>
                </div>
              )}
            </Droppable>
          </div>
          <div className=" w-full justify-center flex">
            <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
              <Button
                fullWidth
                color={"yellow"}
                onClick={handleSubmit}
                disabled={isSubmitted}
              >
                Submit
              </Button>
            </div>
          </div>
        </div>
      </div>
    </DragDropContext>
  );
}
