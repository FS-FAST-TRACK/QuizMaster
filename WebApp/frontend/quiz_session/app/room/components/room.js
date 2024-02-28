"use client";

import React from "react";
import RoomPin from "./roomPin";
import Participants from "./participants";
import Start from "./start";
import useUserTokenData from "@/app/util/useUserTokenData";

export default function Room() {
  return (
    <div className=" w-full h-full p-5 flex flex-col">
      <RoomPin />
      <div className=" grow flex items-center flex-col flex-1   overflow-hidden">
        <Participants />
      </div>
      <Start />
      <div className="flex bottom-0 h-20 items-center justify-center">
        <p className="font-regular text-white">
          Waiting for the host to start...
        </p>
      </div>
    </div>
  );
}
