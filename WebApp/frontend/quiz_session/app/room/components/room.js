import React from "react";
import logo1 from "@/public/logo/Logo1.svg";
import Image from "next/image";
import copy from "@/public/icons/copy.png";
import { Loader, Button } from "@mantine/core";
import { names } from "../util/data";
import RoomPin from "./roomPin";

export default function Room() {
  return (
    <div className=" w-full h-full p-5 flex flex-col">
      <RoomPin />
      <div className=" grow flex items-center flex-col flex-1   overflow-hidden">
        <div className=" flex  flex-col items-center space-y-2">
          <Loader color="rgba(255, 255, 255, 1)" size="md" />
          <div className="text-white font-bold">
            Waiting for other players...
          </div>
        </div>
        <div className="flex flex-wrap justify-center w-full  overflow-auto ">
          {names.map((name, index) => (
            <div
              key={index}
              className="bg-white py-2 px-5 rounded-lg text-green_text font-bold m-2"
            >
              {name}
            </div>
          ))}
        </div>
      </div>
      <div className="flex bottom-0 h-20 items-center justify-center">
        <Button variant="filled" color="yellow">
          Start Quiz
        </Button>
      </div>
    </div>
  );
}
