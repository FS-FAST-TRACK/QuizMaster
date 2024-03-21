import React from "react";
import user from "@/public/icons/user.png";
import Image from "next/image";
import { AnimateSlideInFromBottom } from "./AnimateSlideInFromBottom";

export default function Leaderboard({ leaderBoard }) {
  const topScores = [
    ...new Set(leaderBoard.flatMap((participant) => participant.score)),
  ].slice(0, 3);

  console.log(topScores);

  const firstScore = topScores[0];
  const secondScore = topScores[1];
  const thirdScore = topScores[2];

  const getRankColorClass = (rank) => {
    switch (rank) {
      case 1:
        return "bg-gold";
      case 2:
        return "bg-silver";
      case 3:
        return "bg-bronze";
      default:
        return "bg-blue-200";
    }
  };

  const Participant = ({ participant }) => {
    const { name, score } = participant;
    const isFirst = score == firstScore;
    const isSecond = score == secondScore;
    const isThird = score == thirdScore;

    return (
      <div
        className={` ${getRankColorClass(
          isFirst ? 1 : isSecond ? 2 : isThird ? 3 : 0
        )} flex flex-row p-2 rounded-md`}
      >
        <div className=" flex-grow font-bold">
          <span>{isFirst ? "🏆" : isSecond ? "🥈" : isThird ? "🥉" : ""}</span>
          {" " + name}
        </div>
        <div className="text-score_result">{score}</div>
      </div>
    );
  };

  return (
    <div className="h-full w-full flex justify-center items-center ">
      <AnimateSlideInFromBottom className="w-[90%] sm:w-[75%] md:w-[45%] lg:w-[50%] xl:w-1/3 h-3/4 max-w-[460px] bg-white  rounded-lg shadow-2xl shadow-green-600 overflow-y-scroll">
        <div className="p-5 overflow-auto space-y-5">
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
              <Participant participant={participant} key={index} />
            ))}
          </div>
        </div>
      </AnimateSlideInFromBottom>
    </div>
  );
}
