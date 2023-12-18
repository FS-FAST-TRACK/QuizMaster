"use client";

import React, { useEffect } from "react";
import { conversation } from "../util/data";
import { useState } from "react";
import { useConnection } from "@/app/util/store";

export default function Chats() {
  const username = localStorage.getItem("username");
  const { connection } = useConnection();
  const [conversation, setConverstaion] = useState([]);

  useEffect(() => {
    // Define the event handler function
    const handleChat = (chat) => {
      setConverstaion((prev) => [...prev, chat]);
    };

    // Subscribe to the "chat" event
    connection.on("chat", handleChat);

    // Clean up the subscription when the component unmounts
    return () => {
      connection.off("chat", handleChat);
    };
  }, [connection]);

  return (
    <>
      {conversation.map((message, index) => {
        if (message.name.toLowerCase() === username) {
          return (
            <div
              key={index}
              className="flex flex-col justify-end w-full items-end "
            >
              <div className="text-gray_text text-sm">{message.name}</div>
              <div
                className="bg-wall w-fit  p-1 rounded-lg text-white"
                style={{ maxWidth: "80%" }}
              >
                {message.message}
              </div>
            </div>
          );
        } else if (message.name === "bot") {
          return (
            <div key={index} className="flex flex-col w-full items-center">
              <div className=" text-gray_text text-sm">{message.message}</div>
            </div>
          );
        } else {
          return (
            <div key={index} className="flex flex-col justify-end w-full ">
              <div className="text-gray_text text-sm">{message.name}</div>
              <div
                className="bg-white w-fit  p-1 rounded-lg "
                style={{ maxWidth: "80%" }}
              >
                {message.message}
              </div>
            </div>
          );
        }
      })}
    </>
  );
}
