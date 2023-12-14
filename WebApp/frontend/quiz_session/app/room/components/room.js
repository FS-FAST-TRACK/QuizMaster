import React from "react";
import logo1 from "@/public/logo/Logo1.svg";
import Image from "next/image";
import copy from "@/public/icons/copy.png";
import { Loader, Button } from "@mantine/core";
import { names } from "../util/data";

export default function Room() {
  return (
    <div className=" w-full h-full p-5 flex flex-col">
      <div className=" flex-row flex h-14 space-x-2">
        <div className="bg-white p-2 rounded-lg h-full justify-center items-center flex">
          <Image src={logo1} height={30} />
        </div>
        <div className="flex h-full w-fit flex-col justify-between">
          <h6 className="text-white text-sm">Room Code</h6>
          <div className="flex flex-row  w-full space-x-2">
            <h6 className="text-white text-2xl font-bold">124776</h6>
            <Image src={copy} width={30} height={5} />
          </div>
        </div>
      </div>
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
