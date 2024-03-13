"use client";

import React from "react";
import RoomPin from "../../components/roomPin";
import { useEffect, useState } from "react";
import { useQuestion, useMetaData } from "@/app/util/store";
import { timeFormater } from "@/app/auth/util/handlers";
import useSound from "use-sound";
import { IconVolume, IconVolumeOff } from "@tabler/icons-react";

export default function Header() {
  const { question } = useQuestion();
  const { metadata } = useMetaData();
  const [time, setTime] = useState();
  const [isMute, setIsMute] = useState();

  const [play, { stop }] = useSound(
    "/audio/quiz_master-ten-seconds-count-down.mp3",
    { volume: isMute ? 0 : 0.5 }
  );

  const toggleMute = () => {
    setIsMute((prev) => !prev);
  };

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
      <div className="flex flex-row  w-full justify-between items-center">
        <div className="flex flex-1">
          <RoomPin />
        </div>
        <div className="flex justify-center">
          <div className="justify-center items-center">
            <p className="text-white text-sm mb-2">
              {metadata?.currentSetName}
            </p>
            <div className="flex-row flex bg-white py-2 rounded-md text-green_text flex-2 justify-center items-center">
              <div className="text-base font-bold">
                Question {metadata?.currentQuestionIndex}{" "}
              </div>
              <div>&nbsp; out of {metadata?.totalNumberOfQuestions}</div>
            </div>
          </div>
        </div>
        <div className="flex flex-1 justify-end">
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
      <div
        className="p-4 bg-green-700/50 hover:bg-green-700 rounded-full cursor-pointer absolute mt-3 right-5"
        onClick={toggleMute}
      >
        {isMute ? (
          <IconVolumeOff size={24} color="white" />
        ) : (
          <IconVolume size={24} color="white" />
        )}
      </div>
    </div>
  );
}
