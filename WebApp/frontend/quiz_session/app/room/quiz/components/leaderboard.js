import React from "react";
import user from "@/public/icons/user.png";
import Image from "next/image";

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
        )} flex flex-row p-2 rounded-lg`}
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
      <div className="w-1/4 h-3/4 bg-white p-5 overflow-auto space-y-5 rounded-lg">
        <div className="flex flex-row justify-between">
          <div className="font-bold">Leaderboard</div>
          <div className="flex flex-row space-x-1">
            <Image src={user} width={25} height={20} />
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
    </div>
  );
}
