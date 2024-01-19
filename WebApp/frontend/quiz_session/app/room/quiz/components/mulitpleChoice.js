"use client";
import React, { useState } from "react";
import { Button } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";

export default function MulitpleChoice({ question, connectionId }) {
  const [pick, setPick] = useState();
  const [isSubmitted, setIsSubmitted] = useState(false);
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
  return (
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="text-white">Multiple Choice</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}.
        </div>
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
