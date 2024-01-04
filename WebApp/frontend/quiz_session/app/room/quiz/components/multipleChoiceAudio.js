import React from "react";
import { Button } from "@mantine/core";

export default function MultipleChoiceAudio() {
  return (
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <div className="flex flex-col items-center flex-grow justify-center ">
        <div className="text-white">Multiple Choice Audio</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          Which data structure is typically used to implement a
          Last-In-First-Out (LIFO) behavior?
        </div>
        <audio controls />
      </div>
      <div className="w-full grid grid-cols-2 place-content-center ">
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Queue
        </div>
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Linked List
        </div>
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Tree
        </div>
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Stack
        </div>
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
