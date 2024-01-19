"use client";
import React, { useState } from "react";
import { Button, Slider } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";

export default function SliderPuzzle({ question, connectionId }) {
  const [answer, setAnswer] = useState("0");
  const [isSubmitted, setIsSubmitted] = useState(false);

  const details = question?.details;
  const min = parseInt(details[0].qDetailDesc, 10);
  const max = parseInt(details[1].qDetailDesc, 10);
  const interval = parseInt(details[2].qDetailDesc, 10);

  const handleSubmit = () => {
    let id = question.question.id;
    setIsSubmitted(true);
    submitAnswer({ id, answer: answer.toString(), connectionId });
  };
  return (
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <div className="flex flex-col items-center  w-full ">
        <div className="text-white">Slider</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}
        </div>
      </div>

      <div className="flex-grow w-full text-white text-xl font-bold space-x-2 flex-col flex justify-center">
        <div className="w-full flex justify-center items-center  flex-col ">
          <div>Your Answer</div>

          <div className="w-full flex justify-center items-center text-3xl font-bold ">
            {answer}
          </div>
        </div>

        <div className="flex flex-row items-center ">
          <div>{min}</div>
          <Slider
            color="rgba(209, 209, 209, 1)"
            labelAlwaysOn
            min={min}
            max={max}
            step={interval}
            className="w-full p-20"
            size="xl"
            defaultValue={min}
            onChangeEnd={setAnswer}
            disabled={isSubmitted}
          />
          <div>{max}</div>
        </div>
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
