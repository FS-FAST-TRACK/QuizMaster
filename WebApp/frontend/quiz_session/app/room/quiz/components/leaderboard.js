import React from "react";
import user from "@/public/icons/user.png";
import Image from "next/image";

export default function Leaderboard({ leaderBoard }) {
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
          {leaderBoard.map((score, index) => (
            <div className="bg-gold flex flex-row p-2 rounded-lg" key={index}>
              <div className=" w-10">{index + 1}</div>
              <div className=" flex-grow">{score.name}</div>
              <div>{score.score}</div>
            </div>
          ))}
          {/* <div className="bg-gold flex flex-row p-2 rounded-lg">
              <div className=" w-10">1</div>
              <div className=" flex-grow">Harold</div>
              <div>300</div>
            </div>
            <div className="bg-silver flex flex-row p-2 rounded-lg">
              <div className=" w-10">1</div>
              <div className=" flex-grow">Harold</div>
              <div>300</div>
            </div>
            <div className="bg-bronze flex flex-row p-2 rounded-lg">
              <div className=" w-10">1</div>
              <div className=" flex-grow">Harold</div>
              <div>300</div>
            </div>
            <div className=" flex flex-row p-2 rounded-lg">
              <div className=" w-10">1</div>
              <div className=" flex-grow">Harold</div>
              <div>300</div>
            </div>
            <div className=" flex flex-row p-2 rounded-lg">
              <div className=" w-10">1</div>
              <div className=" flex-grow">Harold</div>
              <div>300</div>
            </div>
            <div className=" flex flex-row p-2 rounded-lg">
              <div className=" w-10">1</div>
              <div className=" flex-grow">Harold</div>
              <div>300</div>
            </div> */}
        </div>
      </div>
    </div>
  );
}
