import Image from "next/image";
import React from "react";
import person from "@/public/icons/user.png";
import { names } from "../util/data";
import { Input } from "@mantine/core";
import send from "@/public/icons/send.png";
import Chats from "./chats";

export default function Chat() {
  return (
    <div className="flex flex-col w-full h-full ">
      <div className="h-14 items-center flex justify-between p-2 border-b-2 border-gray-300">
        <div className="font-bold">Lobby Chat</div>
        <div className="flex flex-row h-full items-center space-x-2">
          <Image src={person} width={30} height={30} />
          <div className="font-bold">{names.length}</div>
        </div>
      </div>
      <div className=" grow px-2 overflow-auto bg-gray-100 space-y-2 pb-2">
        <Chats />
      </div>
      <div className="h-14  p-2 flex flex-row items-center space-x-2 border-t-2 border-gray-300">
        <Input className="w-full" placeholder="Message" />
        <Image
          src={send}
          width={35}
          height={35}
          className="p-2 bg-wall rounded-md"
        />
      </div>
    </div>
  );
}
