import React from "react";
import { Button, Slider } from "@mantine/core";

export default function SliderPuzzle() {
  return (
    <div className="w-full flex flex-col h-full p-5 flex-grow">
      <div className="flex flex-col items-center flex-grow w-full">
        <div className="text-white">Slider</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center  ">
          In what year was the programming language Python first released?
        </div>
      </div>

      <div className="flex flex-row items-center flex-grow w-full text-black space-x-2">
        <div>1989</div>
        <Slider
          color="rgba(209, 209, 209, 1)"
          labelAlwaysOn
          min={1989}
          max={2000}
          className="w-full p-20"
          size="xl"
        />
        <div>2000</div>
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
