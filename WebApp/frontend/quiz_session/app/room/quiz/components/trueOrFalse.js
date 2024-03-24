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
import { useAnswer, useMetaData } from "@/app/util/store";
import { notifications } from "@mantine/notifications";
import Participants from "../../components/participants";
import { useContext } from "react";
import { SoundEffectsContext } from "../../contexts/SoundEffectsContext";
import { useAnswerSFX } from "../../hooks/useAnswerSFX";
import { isCorrectAnswer } from "@/app/util/questionAnswerUtil";
import { AnimateSlideInFromBottom } from "./AnimateSlideInFromBottom";
import { IconX } from "@tabler/icons-react";

export default React.forwardRef(TrueOrFalse);

function TrueOrFalse({ question, connectionId }, ref) {
  const { isAdmin } = useUserTokenData();
  const { answer: ANSWER } = useAnswer();
  const { metadata } = useMetaData();

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

  const { volume, isMute } = useContext(SoundEffectsContext);
  const { playCorrect, playIncorrect } = useAnswerSFX(
    isMute ? 0 : volume / 100
  );

  /* Shows/hides question details during buffer time */
  const showDetails = question?.remainingTime <= question?.question?.qTime;

  const submitScreenshot = (id, connectionId) =>
    takeScreenShot(ref.current).then((image) =>
      uploadScreenshot(image, id, connectionId)
    );

  const handleSubmit = () => {
    let id = question.question.id;
    if (!pick && !isAdmin && !ANSWER) {
      notifications.show({ title: "Please select an option" });
      return;
    }
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
    // handleSubmit if answer is shown
    if (ANSWER && !isSubmitted && !isAdmin) {
      if (!pick) {
        notifications.show({ title: "You have not selected any choices" });
        handleSubmit();
      }
    }
    if (ANSWER && !isAdmin) {
      if (isCorrectAnswer(pick, ANSWER + "")) {
        playCorrect();
      } else {
        playIncorrect();
      }
    }
  }, [ANSWER]);

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
        <div className="mb-2 text-white px-4 py-2 text-sm font-regular border-2 border-white rounded-full">
          True or False
        </div>
        <div className="mb-4">
          <p className="text-sm text-white">{`${
            metadata?.currentDifficulty
          } • ${
            metadata?.points[metadata?.currentDifficulty.toLowerCase()] || 1
          } ${
            metadata?.points[metadata?.currentDifficulty.toLowerCase()] > 1
              ? "points"
              : "point"
          }`}</p>
        </div>
        <div className="text-white font-semibold flex flex-wrap text-center sm:text-2xl md:text-3xl lg:text-text-4xl mb-4 h-52 items-center select-none">
          {question?.question.qStatement}
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
      </div>

      {isAdmin
        ? showDetails && (
            <div className="w-full place-content-center">
              {ANSWER && (
                <AnimateSlideInFromBottom>
                  <div className="py-8 px-[20%]">
                    <p className="text-white">Correct answer is: </p>
                    <div className="border-2 bg-white text-dark_green flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg">
                      <p className="px-4">{ANSWER}</p>
                      <CheckIcon width={20} height={20} />
                    </div>
                  </div>
                </AnimateSlideInFromBottom>
              )}
              <AnimateSlideInFromBottom>
                <div
                  className={`{w-full grid grid-cols-2 place-content-center gap-3 mt-8 ${
                    ANSWER && "opacity-50"
                  }`}
                >
                  <div
                    className={` ${
                      pick === "true"
                        ? "bg-dark_green text-white"
                        : "bg-white text-dark_green"
                    } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                      isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
                    } `}
                    onClick={() => {
                      if (isAdmin) return;
                      if (isSubmitted) return;
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
                      if (isSubmitted) return;
                      handlePick("false");
                    }}
                  >
                    False
                  </div>
                </div>
              </AnimateSlideInFromBottom>
            </div>
          )
        : showDetails && (
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
              <AnimateSlideInFromBottom>
                <div
                  className={`{w-screen grid grid-cols-2 place-content-center gap-3 mt-8`}
                >
                  <div
                    className={` ${
                      pick === "true"
                        ? "bg-dark_green text-white"
                        : "bg-white text-dark_green"
                    } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                      isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
                    } ${ANSWER && pick != "true" ? "opacity-50" : ""}  ${
                      ANSWER &&
                      pick === "true" &&
                      isCorrectAnswer(pick, ANSWER + "")
                        ? "bg-white !border-4 !border-dark_green !text-dark_green !font-semibold"
                        : ANSWER &&
                          pick === "true" &&
                          "bg-white !border-4 !border-red-500 !text-red-500 !font-semibold"
                    }`}
                    onClick={() => {
                      if (isAdmin) return;
                      if (isSubmitted) return;
                      handlePick("true");
                    }}
                  >
                    <span>True</span>
                    {ANSWER &&
                    pick === "true" &&
                    isCorrectAnswer(pick, ANSWER + "") ? (
                      <span className="ml-2">
                        <CheckIcon
                          width={20}
                          height={20}
                          color="rgb(21 128 61)"
                        />
                      </span>
                    ) : (
                      ANSWER &&
                      pick === "true" && (
                        <span className="ml-2">
                          <IconX
                            width={24}
                            height={24}
                            color="red"
                            stroke={3}
                          />
                        </span>
                      )
                    )}
                  </div>
                  <div
                    className={` ${
                      pick === "false"
                        ? "bg-dark_green text-white"
                        : "bg-white text-dark_green"
                    } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
                      isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
                    } ${ANSWER && pick != "false" ? "opacity-50" : ""}
                     ${
                       ANSWER &&
                       pick === "false" &&
                       isCorrectAnswer(pick, ANSWER + "")
                         ? "bg-white !border-2 !border-green-700 !text-green-700 !font-semibold"
                         : ANSWER &&
                           pick === "false" &&
                           "bg-white !border-2 !border-red-500 !text-red-500 !font-semibold"
                     }`}
                    onClick={() => {
                      if (isAdmin) return;
                      if (isSubmitted) return;
                      handlePick("false");
                    }}
                  >
                    <span>False</span>
                    {ANSWER &&
                    pick === "false" &&
                    isCorrectAnswer(pick, ANSWER + "") ? (
                      <span className="ml-2">
                        <CheckIcon
                          width={20}
                          height={20}
                          color="rgb(21 128 61)"
                        />
                      </span>
                    ) : (
                      ANSWER &&
                      pick === "false" && (
                        <span className="ml-2">
                          <IconX
                            width={24}
                            height={24}
                            color="red"
                            stroke={3}
                          />
                        </span>
                      )
                    )}
                  </div>
                </div>
              </AnimateSlideInFromBottom>
            </div>
          )}

      {!isAdmin && showDetails && (
        <AnimateSlideInFromBottom delay="2000">
          <div
            className={`w-full justify-center flex mt-8 ${
              ANSWER ? "opacity-50" : ""
            } ${ANSWER ? "hidden" : ""}`}
          >
            <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
              <Button
                fullWidth
                color={"yellow"}
                size="xl"
                disabled={isSubmitted || ANSWER}
                onClick={handleSubmit}
                className={`shadow-lg ${
                  isSubmitted
                    ? "bg-[#FFAB3E] text-[#FFF9DF] opacity-50"
                    : "bg-[#FF6633]"
                }`}
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
