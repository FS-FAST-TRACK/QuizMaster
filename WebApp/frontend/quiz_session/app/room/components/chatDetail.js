"use client";
import React from "react";
import person from "@/public/icons/user.png";
import Image from "next/image";
import { useParticipants } from "@/app/util/store";
import { IconUsers } from "@tabler/icons-react";

export default function ChatDetail() {
  const { participants } = useParticipants();
  return (
    <div className="flex flex-row h-full items-center space-x-2">
      <IconUsers size={24} />
      <div className="font-semibold">
        {participants.length !== 0 ? participants.length - 1 : 0}
      </div>
    </div>
  );
}
