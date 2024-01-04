"use client";

import React from "react";
import { Input } from "@mantine/core";
import Image from "next/image";
import send from "@/public/icons/send.png";
import { useState } from "react";
import { Button } from "@mantine/core";
import { ActionIcon } from "@mantine/core";
import { IconSend } from "@tabler/icons-react";
import { useConnection } from "@/app/util/store";
import { useSearchParams } from "next/navigation";

export default function Message() {
  const [message, setMessage] = useState();
  const { connection } = useConnection();
  const searchParams = useSearchParams();
  const roomPin = new URLSearchParams(searchParams).get("roomPin");

  const handleSendMessage = (e) => {
    e.preventDefault();
    connection.invoke("Chat", message, roomPin);
  };
  return (
    <div className="h-14  p-2 flex flex-row items-center space-x-2 border-t-2 border-gray-300">
      <Input
        className="w-full"
        placeholder="Message"
        onChange={(e) => setMessage(e.target.value)}
      />
      <ActionIcon onClick={handleSendMessage}>
        <IconSend />
      </ActionIcon>
    </div>
  );
}
