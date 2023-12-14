import React from "react";
import { conversation } from "../util/data";

export default function Chats() {
  return (
    <>
      {conversation.map((message, index) => {
        if (message.role === "Admin") {
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
        } else if (message.role === "Bot") {
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
