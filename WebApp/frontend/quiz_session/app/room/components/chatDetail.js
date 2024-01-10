"use client";
import React from "react";
import person from "@/public/icons/user.png";
import Image from "next/image";
import { useParticipants } from "@/app/util/store";

export default function ChatDetail() {
  const { participants } = useParticipants();
  return (
    <div className="flex flex-row h-full items-center space-x-2">
      <Image src={person} width={30} height={30} />
      <div className="font-bold">{participants.length}</div>
    </div>
  );
}
