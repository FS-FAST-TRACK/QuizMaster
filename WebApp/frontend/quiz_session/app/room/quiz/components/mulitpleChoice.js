"use client";
import React, { useState, useEffect } from "react";
import { Button, CheckIcon, CloseIcon } from "@mantine/core";
import { downloadImage, submitAnswer, uploadScreenshot } from "@/app/util/api";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import QuestionImage from "./questionImage";
import Participants from "../../components/participants";
import useUserTokenData from "@/app/util/useUserTokenData";
import { useScreenshot } from "use-react-screenshot";
import { useAnswer, useMetaData } from "@/app/util/store";
import { notifications } from "@mantine/notifications";
import { useContext } from "react";
import { SoundEffectsContext } from "../../contexts/SoundEffectsContext";
import { useAnswerSFX } from "../../hooks/useAnswerSFX";
import { isCorrectAnswer } from "@/app/util/questionAnswerUtil";
import { IconX } from "@tabler/icons-react";
import { AnimateSlideInFromBottom } from "./AnimateSlideInFromBottom";

export default React.forwardRef(MulitpleChoice);

function MulitpleChoice({ question, connectionId }, ref) {
  const { isAdmin } = useUserTokenData();
  const { metadata } = useMetaData();
  const [pick, setPick] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [opened, { open, close }] = useDisclosure(false);
  const [hasImage, setHasImage] = useState(false);
  const [previousStatement, setPreviousStatement] = useState(null);
  const { answer } = useAnswer();

  /* Shows/hides question details during buffer time */
  const showDetails = question?.remainingTime <= question?.question?.qTime;

  const [image, takeScreenShot] = useScreenshot({
    type: "image/jpeg",
    quality: 1.0,
  });

  const { volume, isMute } = useContext(SoundEffectsContext);
  const { playCorrect, playIncorrect } = useAnswerSFX(
    isMute ? 0 : volume / 100
  );

  const submitScreenshot = (id, connectionId) =>
    takeScreenShot(ref.current).then((image) =>
      uploadScreenshot(image, id, connectionId)
    );

  const handleSubmit = () => {
    let id = question.question.id;
    if (!pick && !isAdmin && !answer) {
      notifications.show({ title: "Please select an option" });
      return;
    }
    setIsSubmitted(true);
    submitAnswer({ id, answer: pick, connectionId });
    submitScreenshot(id, connectionId);
  };

  const handlePick = (answer) => {
    if (!isSubmitted) {
      setPick(answer);
    }
  };

  useEffect(() => {
    // handleSubmit if answer is shown
    if (answer && !isSubmitted && !isAdmin) {
      if (!pick) {
        notifications.show({ title: "You have not selected any choices" });
      }
      handleSubmit();
    }
    if (answer && !isAdmin) {
      if (isCorrectAnswer(pick, answer + "")) {
        playCorrect();
      } else {
        playIncorrect();
      }
    }
  }, [answer]);

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
      setPick(undefined);
      setIsSubmitted(false);
      setImageUrl(undefined);
      setHasImage(false);
      setPreviousStatement(question?.question.qStatement);
    }
  }, [question?.question.qStatement]);

  return (
    <div className="w-full flex flex-col p-5 flex-grow mt-8 bg-green-600">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center flex-grow justify-center">
        <div className="mb-2 text-white px-4 py-2 text-sm font-regular border-2 border-white rounded-full">
          Multiple Choice
        </div>
        <div className="mb-4">
          <p className="text-sm text-white">{`${
            metadata?.currentDifficulty
          } • ${
            metadata?.points[metadata?.currentDifficulty.toLowerCase()] || 0
          } ${
            metadata?.points[metadata?.currentDifficulty.toLowerCase()] > 1
              ? "points"
              : "point"
          }`}</p>
        </div>
        <div className="text-white font-semibold flex flex-wrap text-center sm:text-2xl md:text-3xl lg:text-text-4xl h-52 items-center select-none">
          {question?.question.qStatement}
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>
      {isAdmin ? (
        <div className="w-full grid grid-cols-2 place-content-center">
          <div className="col-span-2">
            {answer && (
              <AnimateSlideInFromBottom>
                <div className="py-8 px-[20%]">
                  <p className="text-white">Correct answer is: </p>
                  <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg rounded-md">
                    <p className="px-4">{answer}</p>
                    <CheckIcon width={20} height={20} />
                  </div>
                </div>
              </AnimateSlideInFromBottom>
            )}
          </div>
        </div>
      ) : (
        <div className="w-full grid grid-cols-2 place-content-center">
          <div className="col-span-2">
            {answer && (
              <AnimateSlideInFromBottom>
                <div className="py-8 px-[20%]">
                  <p className="text-white">Correct answer is: </p>
                  <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg rounded-md">
                    <p className="px-4">{answer}</p>
                    <CheckIcon width={20} height={20} />
                  </div>
                </div>
              </AnimateSlideInFromBottom>
            )}
          </div>
        </div>
      )}

      {showDetails && (
        <AnimateSlideInFromBottom>
          <div
            className={`w-full grid grid-cols-2 place-content-center gap-3 mt-8`}
          >
            {question?.details.map((choices, index) => (
              <div
                className={`${
                  pick === choices.qDetailDesc
                    ? "bg-dark_green text-white"
                    : "bg-white text-dark_green"
                } flex justify-center items-center space-x-3 text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                  isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
                } ${answer && pick != choices.qDetailDesc ? "opacity-50" : ""}
                  ${
                    answer && pick == answer && pick == choices.qDetailDesc
                      ? "!bg-white !text-green-700 !border-green-700 !border-4"
                      : answer && pick == choices.qDetailDesc
                      ? "!bg-white !text-red-500 !border-4 !border-red-500"
                      : ""
                  }`}
                key={index}
                onClick={() => {
                  if (isAdmin) return;
                  if (isSubmitted) return;
                  handlePick(choices.qDetailDesc);
                }}
              >
                <span>{choices.qDetailDesc}</span>
                {answer && pick == answer && pick == choices.qDetailDesc ? (
                  <CheckIcon width={20} height={20} color="rgb(21 128 61)" />
                ) : (
                  answer &&
                  pick == choices.qDetailDesc && (
                    <span className="w-[20px] h-[20px]">
                      <IconX
                        width={24}
                        height={24}
                        color="rgb(239 68 68)"
                        stroke={3}
                      />
                    </span>
                  )
                )}
              </div>
            ))}
          </div>
        </AnimateSlideInFromBottom>
      )}
      {!isAdmin && showDetails && (
        <AnimateSlideInFromBottom delay="2000">
          <div
            className={`w-full justify-center flex mt-8 ${
              answer ? "opacity-50" : ""
            }`}
          >
            <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
              <Button
                fullWidth
                className={`shadow-lg ${
                  isSubmitted
                    ? "bg-[#FFAB3E] text-[#FFF9DF] opacity-50"
                    : "bg-[#FF6633]"
                } ${answer ? "hidden" : ""}`}
                color={"yellow"}
                onClick={handleSubmit}
                size="lg"
                disabled={isSubmitted || answer}
              >
                Submit
              </Button>
            </div>
          </div>
        </AnimateSlideInFromBottom>
      )}
      {isAdmin && (
        <div className="py-8 px-[20%] w-full">
          <Participants excludeAdmins={true} includeLoaderModal={false} />
        </div>
      )}
    </div>
  );
}
