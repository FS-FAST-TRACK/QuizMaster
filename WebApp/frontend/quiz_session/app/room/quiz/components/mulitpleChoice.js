import React from "react";
import { Button } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";

export default function MulitpleChoice({ question, connectionId }) {
  const handleSubmit = (answer) => {
    let id = question.question.id;

    submitAnswer({ id, answer, connectionId });
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
            className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg"
            key={index}
            onClick={() => handleSubmit(choices.qDetailDesc)}
          >
            {choices.qDetailDesc}
          </div>
        ))}
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
          <Button fullWidth color={"yellow"}>
            Sumbit
          </Button>
        </div>
      </div>
    </div>
  );
}
