"use client";
import React, { useState, useEffect } from "react";
import { Button } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";

export default React.forwardRef(TrueOrFalse);

function TrueOrFalse({ question, connectionId }, ref) {
  const [pick, setPick] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [hasImage, setHasImage] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [previousStatement, setPreviousStatement] = useState(null);
  const [opened, { open, close }] = useDisclosure(false);

  const download = (image, { name = "img", extension = "jpg" } = {}) => {
    const a = document.createElement("a");
    a.href = image;
    a.download = createFileName(extension, name);
    a.click();
  };

  const downloadScreenshot = () => takeScreenShot(ref.current).then(download);

  const handleSubmit = () => {
    let id = question.question.id;
    setIsSubmitted(true);
    downloadScreenshot();
    submitAnswer({ id, answer: pick, connectionId });
  };

  const handlePick = (answer) => {
    if (!isSubmitted) {
      setPick(answer);
    }
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
      setPick();
      setIsSubmitted(false);
      setImageUrl();
      setHasImage(false);
      setPreviousStatement(question?.question.qStatement);
    }
  }, [question?.question.qStatement]);

  return (
    <div className="w-full flex flex-col  h-full bg-green-600 p-5 ">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="text-white">True or False</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}.
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>

      <div className="w-full grid grid-cols-2 place-content-center ">
        <div
          className={` ${
            pick === "true"
              ? "bg-dark_green text-white"
              : "bg-white text-dark_green"
          } flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg ${
            isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
          }`}
          onClick={() => handlePick("true")}
        >
          True
        </div>
        <div
          className={` ${
            pick === "false"
              ? "bg-dark_green text-white"
              : "bg-white text-dark_green"
          } flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg ${
            isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
          }`}
          onClick={() => handlePick("false")}
        >
          False
        </div>
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
          <Button
            fullWidth
            color={"yellow"}
            size="xl"
            disabled={isSubmitted}
            onClick={handleSubmit}
          >
            Submit
          </Button>
        </div>
      </div>
    </div>
  );
}
