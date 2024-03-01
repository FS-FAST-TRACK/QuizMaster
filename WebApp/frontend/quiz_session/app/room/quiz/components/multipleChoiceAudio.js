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
    <div className="w-full flex flex-col p-5 flex-grow mt-8">
      <ImageModal opened={opened} close={close} imageUrl={imageUrl} />
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="mb-4 text-white px-4 py-2 text-sm font-regular border-2 border-white rounded-full">
          Multiple Choice Audio
        </div>
        <div className="text-white font-semibold flex flex-wrap text-center sm:text-2xl md:text-3xl lg:text-text-4xl mb-4 h-52 items-center">
          {question?.question.qStatement}.
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
      <div className="w-full grid grid-cols-2 place-content-center gap-3 mt-8">
        {data?.map((choices, index) => (
          <div
            className={`${
              pick === choices.qDetailDesc
                ? "bg-dark_green text-white"
                : "bg-white text-dark_green"
            } flex justify-center items-center text-xl font-bold px-4 py-4 rounded-md shadow-lg ${
              isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
            }`}
            key={index}
            onClick={() => {
              if (isAdmin) return;
              handlePick(choices.qDetailDesc);
            }}
          >
            {choices.qDetailDesc}
          </div>
        ))}
      </div>
      {!isAdmin && (
        <div className=" w-full justify-center flex mt-8">
          <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
            <Button
              fullWidth
              className="shadow-lg"
              color={"yellow"}
              onClick={handleSubmit}
              disabled={isSubmitted}
              size="lg"
            >
              Submit
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
