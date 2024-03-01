"use client";
import React, { useState, useEffect } from "react";
import { Button, CheckIcon, Input } from "@mantine/core";
import { submitAnswer, uploadScreenshot } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";
import useUserTokenData from "@/app/util/useUserTokenData";
import Participants from "../../components/participants";
import { useScreenshot } from "use-react-screenshot";
import { useAnswer } from "@/app/util/store";

export default React.forwardRef(TypeAnswer);

function TypeAnswer({ question, connectionId }, ref) {
  const { isAdmin } = useUserTokenData();
  const { answer: ANSWER } = useAnswer();

  const [answer, setAnswer] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [hasImage, setHasImage] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [previousStatement, setPreviousStatement] = useState(null);
  const [opened, { open, close }] = useDisclosure(false);
  const [image, takeScreenShot] = useScreenshot({
    type: "image/jpeg",
    quality: 1.0,
  });

  const submitScreenshot = (id, connectionId) =>
    takeScreenShot(ref.current).then((image) =>
      uploadScreenshot(image, id, connectionId)
    );

  const handleSubmit = () => {
    let id = question.question.id;
    setIsSubmitted(true);
    submitScreenshot(id, connectionId);
    submitAnswer({ id, answer, connectionId });
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
      {isAdmin ? (
        <div className="py-8">
          { ANSWER && 
            <div className="py-8 px-[20%]">
              <p className="text-white">Correct answer is: </p>
              <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg"><p className="px-4">{ANSWER}</p><CheckIcon width={20} height={20}/></div>
            </div>
          }
          <Participants includeLoaderModal={false} />
        </div>
      ) : (
        <>
        { ANSWER && 
          <div className="py-8 px-[20%] w-full">
            <p className="text-white">Correct answer is: </p>
            <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg"><p className="px-4">{ANSWER}</p><CheckIcon width={20} height={20}/></div>
          </div>
        }
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
              disabled={isSubmitted || ANSWER}
              className="bg-[#FF6633]"
            >
              Submit
            </Button>
          </div>
        </div>
        </>
      )}

      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg"></div>
      </div>
    </div>
  );
}
