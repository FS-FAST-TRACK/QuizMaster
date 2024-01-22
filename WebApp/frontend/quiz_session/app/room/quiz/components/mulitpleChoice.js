"use client";
import React, { useState, useEffect } from "react";
import { Button } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";
import Image from "next/image";
import { Modal } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import image from "@/public/icons/image.png";

export default function MulitpleChoice({ question, connectionId }) {
  const [pick, setPick] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [imageUrl, setImageUrl] = useState();
  const [opened, { open, close }] = useDisclosure(false);
  const [hasImage, setHasImage] = useState(false);
  const [previousStatement, setPreviousStatement] = useState(null);

  const handleSubmit = () => {
    let id = question.question.id;
    setIsSubmitted(true);
    submitAnswer({ id, answer: pick, connectionId });
  };

  const handlePick = (answer) => {
    if (!isSubmitted) {
      setPick(answer);
    }
  };

  const downloadImage = async (url) => {
    try {
      const authToken = localStorage.getItem("token");

      const response = await fetch(`${url}`, {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      });

      // Check if the request was successful (status code 200)
      if (response.ok) {
        const blob = await response.blob();
        const imageUrl = URL.createObjectURL(blob);

        // Set the image URL in the state or use it directly in the component
        setImageUrl(imageUrl);
      } else {
        // Handle errors
        console.error("Error downloading image:", response.statusText);
      }
    } catch (error) {
      console.error("Error downloading image:", error);
    }
  };

  useEffect(() => {
    if (question?.question.qImage) {
      downloadImage(question.question.qImage);
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
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <Modal
        opened={opened}
        onClose={close}
        withCloseButton={false}
        centered
        size="xl"
      >
        <Image
          src={imageUrl}
          width={0}
          height={0}
          style={{ width: "100%", height: "100%" }}
        />
      </Modal>
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="text-white">Multiple Choice</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}.
        </div>
        {hasImage && (
          <>
            {imageUrl ? (
              <Image
                src={imageUrl}
                width={0}
                height={0}
                sizes="100vw"
                style={{ width: "25%", height: "auto" }}
                onClick={open}
                className="rounded-md"
              />
            ) : (
              <div className=" w-1/4 h-fit bg-dark_green flex justify-center items-center rounded-lg animate-pulse text-white p-2">
                <Image
                  src={image}
                  width={0}
                  height={0}
                  sizes="100vw"
                  className="rounded-md"
                />
              </div>
            )}
          </>
          // <Image
          //   src={imageUrl}
          //   width={0}
          //   height={0}
          //   sizes="100vw"
          //   style={{ width: "25%", height: "auto" }}
          //   onClick={open}
          //   className="rounded-md"
          // />
        )}
        <div className="skel"></div>
      </div>
      <div className="w-full grid grid-cols-2 place-content-center">
        {question?.details.map((choices, index) => (
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
    </div>
  );
}
