"use client";

import React from "react";
import RoomPin from "../../components/roomPin";
import { useEffect, useState } from "react";
import { useConnection } from "@/app/util/store";

export default function Header() {
  const { connection } = useConnection();
  const [time, setTime] = useState();
  useEffect(() => {
    connection.on("question", (question) => {
      console.log(question);
      /*
       question - question.question.qStatement
       answers - question.details.qDetailDesc
       type - question.question.qTypeId

       Type Questions
       1 - Multiple Choice
       2 - Multiple Choice + Audio
       3 - True or False
       4 - Type Answer
       5 - Slider
       6 - Puzzle
      */
      setTime(question.remainingTime);
      console.log(question.remainingTime);
    });
  }, []);
  return (
    <div className="px-5 pt-2 w-full">
      <div className="flex flex-row  w-full">
        <div className="flex justify-start w-full">
          <RoomPin />
        </div>

        <div className="flex justify-center w-full">
          <div className="flex-row flex bg-white items-center px-5 py-2 rounded-lg text-green_text ">
            <div className="text-xl font-bold">Question 1</div>
            <div> out of 10</div>
          </div>
        </div>
        <div className="flex items-end flex-col w-full">
          <div className="text-white">Time left</div>
          <div className="text-2xl font-bold text-white">00:{time}</div>
        </div>
      </div>
    </div>
  );
}
