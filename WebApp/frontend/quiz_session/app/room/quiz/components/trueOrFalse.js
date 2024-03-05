"use client";
import React, { useState, useEffect } from "react";
import { Button, CheckIcon } from "@mantine/core";
import { submitAnswer, uploadScreenshot } from "@/app/util/api";
import { downloadImage } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";
import useUserTokenData from "@/app/util/useUserTokenData";
import { useScreenshot } from "use-react-screenshot";
import { useAnswer } from "@/app/util/store";

export default React.forwardRef(TrueOrFalse);

function TrueOrFalse({ question, connectionId }, ref) {
  const { isAdmin } = useUserTokenData();
  const { answer: ANSWER } = useAnswer();

  const [pick, setPick] = useState();
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
    submitAnswer({ id, answer: pick, connectionId });
  };

  const handlePick = (answer) => {
    if (!isSubmitted) {
      setPick(answer);
    }
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
        <div className="mb-4 text-white px-4 py-2 text-sm font-regular border-2 border-white rounded-full">
          True or False
        </div>
        <div className="text-white font-semibold flex flex-wrap text-center sm:text-2xl md:text-3xl lg:text-text-4xl mb-4 h-52 items-center">
          {question?.question.qStatement}.
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>
      {isAdmin ? (
        <div className="w-full place-content-center">
          {ANSWER && (
            <div className="py-8 px-[20%]">
              <p className="text-white">Correct answer is: </p>
              <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg">
                <p className="px-4">{ANSWER}</p>
                <CheckIcon width={20} height={20} />
              </div>
            </div>
          )}
          <div
            className={`{w-full grid grid-cols-2 place-content-center gap-3 mt-8 ${
              ANSWER ? "opacity-50" : ""
            }`}
          >
            <div
              className={` ${
                pick === "true"
                  ? "bg-dark_green text-white"
                  : "bg-white text-dark_green"
              } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
              }`}
              onClick={() => {
                if (isAdmin) return;
                handlePick("true");
              }}
            >
              True
            </div>
            <div
              className={` ${
                pick === "false"
                  ? "bg-dark_green text-white"
                  : "bg-white text-dark_green"
              } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
              }`}
              onClick={() => {
                if (isAdmin) return;
                handlePick("false");
              }}
            >
              False
            </div>
          </div>
        </div>
      ) : (
        <div className="w-full place-content-center">
          {ANSWER && (
            <div className="py-8 px-[20%] col-span-2">
              <p className="text-white">Correct answer is: </p>
              <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg">
                <p className="px-4">{ANSWER}</p>
                <CheckIcon width={20} height={20} />
              </div>
            </div>
          )}
          <div
            className={`{w-screen grid grid-cols-2 place-content-center gap-3 mt-8 ${
              ANSWER ? "opacity-50" : ""
            }`}
          >
            <div
              className={` ${
                pick === "true"
                  ? "bg-dark_green text-white"
                  : "bg-white text-dark_green"
              } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
              }`}
              onClick={() => {
                if (isAdmin) return;
                handlePick("true");
              }}
            >
              True
            </div>
            <div
              className={` ${
                pick === "false"
                  ? "bg-dark_green text-white"
                  : "bg-white text-dark_green"
              } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
              }`}
              onClick={() => {
                if (isAdmin) return;
                handlePick("false");
              }}
            >
              False
            </div>
          </div>
        </div>
      )}

      {!isAdmin && (
        <div
          className={`w-full justify-center flex mt-8 ${
            ANSWER ? "opacity-50" : ""
          }`}
        >
          <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
            <Button
              fullWidth
              className="shadow-lg"
              color={"yellow"}
              size="xl"
              disabled={isSubmitted || ANSWER}
              onClick={handleSubmit}
              className="bg-[#FF6633]"
            >
              Submit
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
