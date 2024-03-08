"use client";
import React, { useState, useEffect } from "react";
import { Button, CheckIcon, Input } from "@mantine/core";
import { submitAnswer, uploadScreenshot } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure, useForceUpdate } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";
import useUserTokenData from "@/app/util/useUserTokenData";
import Participants from "../../components/participants";
import { useScreenshot } from "use-react-screenshot";
import { useAnswer } from "@/app/util/store";
import { notifications } from "@mantine/notifications";

export default React.forwardRef(TypeAnswer);

function TypeAnswer({ question, connectionId, answer: ANSWER }, ref) {
  const { isAdmin } = useUserTokenData();
  //const { answer: ANSWER } = useAnswer();

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
    if (!ANSWER && !answer && !isAdmin) {
      notifications.show({
        title: "Please type your answer in the input field",
      });
      return;
    }
    setIsSubmitted(true);
    submitScreenshot(id, connectionId);
    submitAnswer({ id, answer, connectionId });
  };

  useEffect(() => {
    if (question?.question.qImage) {
      if (question.question.qImage === "nothing") return;
      downloadImage({
        id: question.question.qImage,
        setImageUrl: setImageUrl,
        setHasImage: setHasImage,
      });
      setHasImage(true);
    }
  }, [question?.question.qImage, previousStatement]);

  useEffect(() => {
    // clear input field if answer is shown
    if (ANSWER && !isSubmitted) {
      if (!answer && !isAdmin) {
        notifications.show({ title: "No answer inputted" });
      }
      setTimeout(() => {
        setAnswer();
      }, 10_000);
      handleSubmit();
    }
  }, [ANSWER]);

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
    <div className="w-full flex flex-col h-full  items-center flex-grow p-5  bg-green-600">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center h-96 justify-center ">
        <div className="mb-4 text-white px-4 py-2 text-sm font-regular border-2 border-white rounded-full">
          Type Answer
        </div>
        <div className="text-white font-semibold flex flex-wrap text-center sm:text-2xl md:text-3xl lg:text-text-4xl mb-4 h-52 items-center select-none">
          {question?.question.qStatement}
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>
      {isAdmin ? (
        <div className="py-8 px-[20%] w-full">
          {ANSWER ? (
            <div className="py-8 px-[20%]">
              <p className="text-white">Correct answer is: </p>
              <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg rounded-md">
                <p className="px-4">{ANSWER}</p>
                <CheckIcon width={20} height={20} />
              </div>
            </div>
          ) : (
            <div className=" flex w-full mt-8 items-center justify-center">
              <div className="border-2 border-white bg-green-700 rounded-md flex items-center  px-6 py-3">
                <div className="animate-pulse w-3">
                  <span className="text-2xl text-white">|</span>
                </div>
                <p className="text-white opacity-50 w-fit text-base font-regular">
                  Type your answers on the text area
                </p>
              </div>
            </div>
          )}
          <Participants excludeAdmins={true} includeLoaderModal={false} />
        </div>
      ) : (
        <>
          {ANSWER && (
            <div className="py-8 px-[20%] w-full">
              <p className="text-white">Correct answer is: </p>
              <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg rounded-md">
                <p className="px-4">{ANSWER}</p>
                <CheckIcon width={20} height={20} />
              </div>
            </div>
          )}
          {!isAdmin && (
            <div className="flex flex-row w-1/2 space-x-2">
              <div className="w-3/4">
                <Input
                  id="input-typeAnswer"
                  placeholder="Type your Answer"
                  size="xl"
                  onChange={(e) => {
                    setAnswer(e.target.value);
                  }}
                  disabled={isSubmitted || ANSWER}
                  value={answer ?? (ANSWER ? "" : "")}
                />
              </div>
              <div className="w-1/4">
                <Button
                  fullWidth
                  color={"yellow"}
                  size="xl"
                  onClick={handleSubmit}
                  disabled={isSubmitted || ANSWER}
                  className={`shadow-lg ${
                    isSubmitted
                      ? "bg-[#FFAB3E] text-[##FFF9DF]"
                      : "bg-[#FF6633]"
                  }`}
                >
                  Submit
                </Button>
              </div>
            </div>
          )}
        </>
      )}

      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg"></div>
      </div>
    </div>
  );
}
