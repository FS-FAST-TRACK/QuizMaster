"use client";

import React from "react";
import RoomPin from "../../components/roomPin";
import { useEffect, useState } from "react";
import { useQuestion, useMetaData } from "@/app/util/store";
import { timeFormater } from "@/app/auth/util/handlers";
import useSound from "use-sound";

export default function Header() {
  const { question } = useQuestion();
  const { metadata } = useMetaData();
  const [time, setTime] = useState();

  const [play, { stop }] = useSound(
    "/audio/quiz_master-ten-seconds-count-down.mp3",
    { volume: 0.5 }
  );

  useEffect(() => {
    setTime(question?.remainingTime);
  }, [question]);

  useEffect(() => {
    if (time === 10) {
      play();
    }
    if (time === 0) {
      stop();
    }
  }, [time]);

  return (
    <div className="px-5 pt-2 w-full bg-green-600 ">
      <div className="flex flex-row  w-full justify-between">
        <div className="flex justify-start">
          <RoomPin />
        </div>
        <div className="flex justify-center">
          <div className="flex-row flex bg-white items-center px-5 py-2 rounded-lg text-green_text ">
            <div className="text-xl font-bold">
              Question {metadata?.currentQuestionIndex}{" "}
            </div>
            <div>&nbsp; out of {metadata?.totalNumberOfQuestions}</div>
          </div>
        </div>
        <div
          className={`flex flex-col rounded-md px-4 py-2 w-28 justify-center items-center ${
            time > 10 ? " bg-green-700/50" : "bg-red-500"
          }`}
        >
          <div className="text-white text-sm">Time left</div>
          <div
            className={`text-2xl font-bold text-white ${
              time <= 5 && time > 0 ? "animate-ping" : ""
            }`}
          >
            {timeFormater(time)}
          </div>
        </div>
      </div>
    </div>
  );
}
