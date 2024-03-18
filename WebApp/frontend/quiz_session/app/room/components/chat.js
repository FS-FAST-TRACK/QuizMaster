import Image from "next/image";
import React from "react";
import person from "@/public/icons/user.png";
import { names } from "../util/data";
import { Input } from "@mantine/core";
import send from "@/public/icons/send.png";
import Chats from "./chats";
import Message from "./message";
import ChatDetail from "./chatDetail";
import { IconChevronRight } from "@tabler/icons-react";

export default function Chat({ onToggleCollapseChat }) {
  return (
    <div className="flex flex-col w-full h-full shadow-lg">
      <div className="h-14 items-center flex justify-between px-4 border-b-2 border-gray-200">
        <div className="flex gap-2 py-4">
          <IconChevronRight
            className="sm:visible md:hidden cursor-pointer hover:bg-slate-100 rounded-full"
            size={24}
            onClick={onToggleCollapseChat}
          />
          <div className="font-semibold">Lobby Chat</div>
        </div>

        <ChatDetail />
      </div>
      <div className=" grow px-2 overflow-auto bg-gray-100 space-y-2 pb-2">
        <Chats />
      </div>
      <Message />
    </div>
  );
}
