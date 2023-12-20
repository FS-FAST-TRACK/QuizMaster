import React from "react";
import { Button } from "@mantine/core";
import { submitAnswer } from "@/app/util/api";

export default function TrueOrFalse({ question, connectionId }) {
  const handleSubmit = (answer) => {
    let id = question.question.id;
    submitAnswer({ id, answer, connectionId });
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
          className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg"
          onClick={() => handleSubmit("true")}
        >
          True
        </div>
        <div
          className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg"
          onClick={() => handleSubmit("false")}
        >
          False
        </div>
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
          <Button fullWidth color={"yellow"} size="xl">
            Sumbit
          </Button>
        </div>
      </div>
    </div>
  );
}
