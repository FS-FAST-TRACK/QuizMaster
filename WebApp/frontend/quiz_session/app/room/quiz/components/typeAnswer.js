import React from "react";
import { Button, Input } from "@mantine/core";

export default function TypeAnswer({ question }) {
  return (
    <div className="w-full flex flex-col h-full  items-center flex-grow p-5">
      <div className="flex flex-col items-center h-96 justify-center ">
        <div className="text-white">Type Answer</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          {question?.question.qStatement}
        </div>
      </div>

      <div className="flex flex-row w-1/2 space-x-2">
        <div className="w-3/4">
          <Input placeholder="Type your Answer" size="xl" />
        </div>
        <div className="w-1/4">
          <Button fullWidth color={"yellow"} size="xl">
            Sumbit
          </Button>
        </div>
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg"></div>
      </div>
    </div>
  );
}
