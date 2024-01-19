"use client";
import React, { useState } from "react";
import { Button } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";

export default function TrueOrFalse({ question, connectionId }) {
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
    <div className="w-full flex flex-col  h-full bg-green-600 p-5 ">
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="text-white">True or False</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}.
        </div>
      </div>

      <div className="w-full grid grid-cols-2 place-content-center ">
        <div
          className={` ${
            pick === "true"
              ? "bg-dark_green text-white"
              : "bg-white text-dark_green"
          } flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg ${
            isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
          }`}
          onClick={() => handlePick("true")}
        >
          True
        </div>
        <div
          className={` ${
            pick === "false"
              ? "bg-dark_green text-white"
              : "bg-white text-dark_green"
          } flex justify-center items-center m-5 text-xl font-bold p-3 shadow-lg ${
            isSubmitted ? "cursor-not-allowed" : "cursor-pointer"
          }`}
          onClick={() => handlePick("false")}
        >
          False
        </div>
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
          <Button
            fullWidth
            color={"yellow"}
            size="xl"
            disabled={isSubmitted}
            onClick={handleSubmit}
          >
            Sumbit
          </Button>
        </div>
      </div>
    </div>
  );
}
