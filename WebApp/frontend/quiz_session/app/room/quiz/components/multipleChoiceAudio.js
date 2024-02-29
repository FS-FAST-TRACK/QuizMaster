"use client";
import React, { useState, useEffect } from "react";
import { Button } from "@mantine/core";
import Image from "next/image";
import audio from "@/public/icons/audio.png";
import { useDisclosure } from "@mantine/hooks";
import ImageModal from "./modal";
import { downloadImage, uploadScreenshot } from "@/app/util/api";
import QuestionImage from "./questionImage";
import { submitAnswer } from "@/app/util/api";
import useUserTokenData from "@/app/util/useUserTokenData";
import Participants from "../../components/participants";
import { useScreenshot } from "use-react-screenshot";

export default React.forwardRef(MultipleChoiceAudio);

function MultipleChoiceAudio({ question, connectionId }, ref) {
  const { isAdmin } = useUserTokenData();

  console.log(question);
  const [data, setData] = useState([]);
  const [pick, setPick] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [opened, { open, close }] = useDisclosure(false);
  const [hasImage, setHasImage] = useState(false);
  const [text, setText] = useState("");
  const [previousStatement, setPreviousStatement] = useState(null);

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

  const speak = () => {
    const utterance = new SpeechSynthesisUtterance(text);
    speechSynthesis.speak(utterance);
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
      console.log(question);
      const options = question?.details;
      console.log(options);

      // Copy all elements except the last 2
      const copiedData = [...options?.slice(0, options?.length - 2)];
      //Get the number of possible answers
      const text = options[options.length - 2];
      setText(text.qDetailDesc);

      setData(copiedData);
      setPick(undefined);
      setIsSubmitted(false);
      setImageUrl(undefined);
      setHasImage(false);
      setPreviousStatement(question?.question.qStatement);
    }
  }, [question?.question.qStatement]);

  return (
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="text-white">Multiple Choice Audio</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}
        </div>
        {hasImage && <QuestionImage imageUrl={imageUrl} open={open} />}
        <Button
          color={"white"}
          leftSection={<Image src={audio} />}
          onClick={speak}
          variant="default"
        >
          <div className="text-wall">Play Audio</div>
        </Button>
      </div>
      {isAdmin ? (
        <Participants includeLoaderModal={false} />
      ) : (
        <div className="w-full grid grid-cols-2 place-content-center">
          {data?.map((choices, index) => (
            <div
              className={`${
                pick === choices.qDetailDesc
                  ? "bg-dark_green text-white"
                  : "bg-white text-dark_green"
              } flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg ${
                isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
              }`}
              key={index}
              onClick={() => handlePick(choices.qDetailDesc)}
            >
              {choices.qDetailDesc}
            </div>
          ))}
        </div>
      )}
      {!isAdmin && (
        <div className=" w-full justify-center flex">
          <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
            <Button
              fullWidth
              color={"yellow"}
              onClick={handleSubmit}
              disabled={isSubmitted}
            >
              Sumbit
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
