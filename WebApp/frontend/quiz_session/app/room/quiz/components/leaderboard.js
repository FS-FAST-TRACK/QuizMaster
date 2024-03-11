import React from "react";
import user from "@/public/icons/user.png";
import Image from "next/image";

export default function Leaderboard({ leaderBoard }) {
  const getRankColorClass = (index) => {
    switch (index) {
      case 0:
        return "bg-gold";
      case 1:
        return "bg-silver";
      case 2:
        return "bg-bronze";
      default:
        return "bg-green-200";
    }
  };
  return (
    <div className="h-full w-full flex justify-center items-center ">
      <div className="w-[90%] sm:w-[75%] md:w-[45%] lg:w-[50%] xl:w-1/3 h-3/4 max-w-[460px] bg-white p-5 overflow-auto space-y-5 rounded-lg shadow-2xl shadow-green-600">
        <div className="flex flex-row justify-between">
          <div className="font-bold">Leaderboard</div>
          <div className="flex flex-row space-x-1">
            <Image src={user} width={23} height={20} />
            <div>{leaderBoard.length}</div>
          </div>
        </div>
        <div className="space-y-3">
          <div className="flex flex-row p-2 rounded-lg">
            <div className=" flex-grow font-bold text-slate-500">
              PARTICIPANTS
            </div>
            <div className="text-score_result">SCORE</div>
          </div>
        </div>
        <div className="space-y-3">
          {leaderBoard.map((participant, index) => (
            <div
              className={` ${getRankColorClass(
                index
              )} flex flex-row p-2 rounded-lg`}
              key={index}
            >
              <div className=" flex-grow font-bold">{participant.name}</div>
              <div className="text-score_result">{participant.score}</div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
