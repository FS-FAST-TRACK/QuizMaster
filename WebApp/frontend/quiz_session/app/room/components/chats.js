"use client";

import React, { useEffect } from "react";
import { useState } from "react";
import { useChat } from "@/app/util/store";
import { useRef } from "react";

export default function Chats() {
  const username = localStorage.getItem("username");
  const { chat } = useChat();
  const [conversation, setConverstaion] = useState([]);
  const lastMessageRef = useRef(null);

  useEffect(() => {
    setConverstaion((prev) => [...prev, chat]);
  }, [chat]);

  useEffect(() => {
    if (lastMessageRef.current) {
      lastMessageRef.current.scrollIntoView({ behavior: "smooth" });
    }
    console.log("Conversations", conversation);
  }, [conversation]);

  return (
    <>
      {conversation?.map((message, index) => {
        if (message?.name.toLowerCase() === username) {
          return (
            <div
              key={index}
              className="flex flex-col justify-end w-full items-end "
              ref={index === conversation.length - 1 ? lastMessageRef : null}
            >
              <div className="text-gray-400 text-xs mb-1">
                {`${message?.name} ${message?.isAdmin ? "(Host)" : ""}`}
              </div>
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
            <div
              key={index}
              className="flex flex-col w-full items-center p-2"
              ref={index === conversation.length - 1 ? lastMessageRef : null}
            >
              <div className="text-gray-400 text-xs">{message?.message}</div>
            </div>
          );
        } else {
          return (
            <div
              key={index}
              className="flex flex-col justify-end w-full"
              ref={index === conversation.length - 1 ? lastMessageRef : null}
            >
              <div>
                <div className="text-gray-300 text-xs mb-1">
                  {`${message?.name} ${message?.isAdmin ? "(Host)" : ""}`}
                </div>
              </div>
              <div
                className="bg-white w-fit  p-1 rounded-md px-4 py-2  shadow-sm"
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
