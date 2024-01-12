"use client";

import React from "react";
import Leaderboard from "./leaderboard";
import { useEffect, useState } from "react";

export default function Interval({ leaderBoard }) {
  const [seconds, setSeconds] = useState(10);
  useEffect(() => {
    const timer = setInterval(() => {
      setSeconds((prevSeconds) => prevSeconds - 1);

      if (seconds === 0) {
        clearInterval(timer);
      }
    }, 1000);

    return () => clearInterval(timer);
  }, [seconds]);
  return (
    <>
      <div className="text-white flex items-center space-x-1 mt-4 flex-col">
        <div>Next round in: </div>
        <div
          className={`${
            seconds <= 5 ? "text-red-500" : "text-white "
          } font-bold  text-4xl`}
        >
          {seconds}
        </div>
      </div>
      <Leaderboard leaderBoard={leaderBoard} />
    </>
  );
}
