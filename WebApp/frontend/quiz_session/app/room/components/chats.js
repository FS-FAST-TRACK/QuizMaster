"use client";

import React, { useEffect } from "react";
import { useState } from "react";
import { useChat } from "@/app/util/store";

export default function Chats() {
  const username = localStorage.getItem("username");
  const { chat } = useChat();
  const [conversation, setConverstaion] = useState([]);

  useEffect(() => {
    setConverstaion((prev) => [...prev, chat]);
  }, [chat]);

  return (
    <>
      {conversation?.map((message, index) => {
        if (message?.name.toLowerCase() === username) {
          return (
            <div
              key={index}
              className="flex flex-col justify-end w-full items-end "
            >
              <div className="text-gray_text text-sm">{message?.name}</div>
              <div
                className="bg-wall w-fit  p-1 rounded-lg text-white px-4 py-2"
                style={{ maxWidth: "80%" }}
              >
                {message?.message}
              </div>
            </div>
          );
        } else if (message?.name === "bot") {
          return (
            <div key={index} className="flex flex-col w-full items-center">
              <div className=" text-gray_text text-sm">{message?.message}</div>
            </div>
          );
        } else {
          return (
            <div key={index} className="flex flex-col justify-end w-full">
              <div className="text-gray_text text-xs">{message?.name}</div>
              <div
                className="bg-white w-fit  p-1 rounded-lg px-4 py-2"
                style={{ maxWidth: "80%" }}
              >
                {message?.message}
              </div>
            </div>
          );
        }
      })}
    </>
  );
}
