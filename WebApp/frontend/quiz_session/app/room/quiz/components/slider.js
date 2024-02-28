"use client";
import React, { useState, useEffect } from "react";
import { Button, Slider } from "@mantine/core";
import { submitAnswer, uploadScreenshot } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";
import { useScreenshot } from "use-react-screenshot";

export default React.forwardRef(SliderPuzzle);

function SliderPuzzle({ question, connectionId }, ref) {
  const [answer, setAnswer] = useState("0");
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [hasImage, setHasImage] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [previousStatement, setPreviousStatement] = useState(null);
  const [opened, { open, close }] = useDisclosure(false);
  const [image, takeScreenShot] = useScreenshot({
    type: "image/jpeg",
    quality: 1.0,
  });

  const submitScreenshot = (id, connectionId) => takeScreenShot(ref.current).then((image) => uploadScreenshot(image, id, connectionId));

  const details = question?.details;
  // const min = parseInt(details[0].qDetailDesc, 10);
  // const max = parseInt(details[1].qDetailDesc, 10);
  // const interval = parseInt(details[2].qDetailDesc, 10);

  const maxValue = details.reduce((acc, curr) => {
    if (parseInt(curr.qDetailDesc, 10) > acc) {
      return parseInt(curr.qDetailDesc, 10);
    }
    return acc;
  }, 0);

  const minValue = details.reduce((acc, curr) => {
    if (parseInt(curr.qDetailDesc, 10) < acc) {
      return parseInt(curr.qDetailDesc, 10);
    }
    return acc;
  }, 0);

  const handleSubmit = () => {
    let id = question.question.id;
    setIsSubmitted(true);
    submitScreenshot(id, connectionId);
    submitAnswer({ id, answer: pick, connectionId });
  };

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
      setAnswer("0");
      setIsSubmitted(false);
      setImageUrl();
      setHasImage(false);
      setPreviousStatement(question?.question.qStatement);
    }
  }, [question?.question.qStatement]);

  return (
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center  w-full ">
        <div className="text-white">Slider</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>

      <div className="flex-grow w-full text-white text-xl font-bold space-x-2 flex-col flex justify-center">
        <div className="w-full flex justify-center items-center  flex-col ">
          <div>Your Answer</div>

          <div className="w-full flex justify-center items-center text-3xl font-bold ">
            {answer}
          </div>
        </div>

        <div className="flex flex-row items-center ">
          <div>{minValue}</div>
          <Slider
            color="rgba(209, 209, 209, 1)"
            labelAlwaysOn
            min={minValue}
            max={maxValue}
            className="w-full p-20"
            size="xl"
            defaultValue={minValue}
            onChangeEnd={setAnswer}
            disabled={isSubmitted}
          />
          <div>{maxValue}</div>
        </div>
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
  );
}
