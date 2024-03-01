"use client";

import React from "react";
import RoomPin from "../../components/roomPin";
import { useEffect, useState } from "react";
import { useQuestion, useMetaData } from "@/app/util/store";
import { timeFormater } from "@/app/auth/util/handlers";

export default function Header() {
  const { question } = useQuestion();
  const { metadata } = useMetaData();
  const [time, setTime] = useState();
  useEffect(() => {
    setTime(question?.remainingTime);
  }, [question]);

  return (
    <div className="px-5 pt-2 w-full">
      <div className="flex flex-row  w-full">
        <div className="flex justify-start w-full">
          <RoomPin />
        </div>

        <div className="flex justify-center w-full">
          <div className="flex-row flex bg-white items-center px-5 py-2 rounded-lg text-green_text ">
            <div className="text-xl font-bold">
              Question {metadata?.currentQuestionIndex}{" "}
            </div>
            <div>&nbsp; out of {metadata?.totalNumberOfQuestions}</div>
          </div>
        </div>
        <div className="flex items-end flex-col w-full">
          <div className="text-white">Time left</div>
          <div className="text-2xl font-bold text-white">
            {timeFormater(time)}
          </div>
        </div>
      </div>
    </div>
  );
}
