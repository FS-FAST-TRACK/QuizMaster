"use client";
import React, { useState, useEffect } from "react";
import { Button, Input } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";

export default React.forwardRef(TypeAnswer);

function TypeAnswer({ question, connectionId }, ref) {
  const [answer, setAnswer] = useState();
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
      setAnswer();
      setIsSubmitted(false);
      setImageUrl();
      setHasImage(false);
      setPreviousStatement(question?.question.qStatement);
    }
  }, [question?.question.qStatement]);

  return (
    <div className="w-full flex flex-col h-full  items-center flex-grow p-5">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center h-96 justify-center ">
        <div className="text-white">Type Answer</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>

      <div className="flex flex-row w-1/2 space-x-2">
        <div className="w-3/4">
          <Input
            placeholder="Type your Answer"
            size="xl"
            onChange={(e) => {
              setAnswer(e.target.value);
            }}
            disabled={isSubmitted}
          />
        </div>
        <div className="w-1/4">
          <Button
            fullWidth
            color={"yellow"}
            size="xl"
            onClick={handleSubmit}
            disabled={isSubmitted}
          >
            Sumbit
          </Button>
        </div>
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg"></div>
      </div>
    </div>
  );
}
