"use client";

import React from "react";
import RoomPin from "./roomPin";
import Participants from "./participants";
import Start from "./start";
import useUserTokenData from "@/app/util/useUserTokenData";
import { IconMessage } from "@tabler/icons-react";

export default function Room({ onToggleCollapseChat }) {
  const { isAdmin } = useUserTokenData();
  return (
    <div className=" w-full h-full p-5 flex flex-col">
      <div className="flex justify-between">
        <RoomPin />
        <div
          className="sm:visible md:hidden p-4 bg-green-700/50 hover:bg-green-700 rounded-full cursor-pointer"
          onClick={onToggleCollapseChat}
        >
          <IconMessage size={24} color="white" />
        </div>
      </div>
      <div className=" grow flex items-center flex-col flex-1 overflow-hidden">
        <Participants />
      </div>
      <Start />
      {!isAdmin && (
        <div className="flex bottom-0 h-20 items-center justify-center animate-pulse">
          <p className="font-regular text-white">
            Waiting for the host to start...
          </p>
        </div>
      )}
    </div>
  );
}
