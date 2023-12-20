"use client";

import React from "react";
import { useEffect, useState } from "react";
import { useConnection } from "@/app/util/store";
import MultipleChoiceAudio from "./multipleChoiceImage";
import MulitpleChoice from "./mulitpleChoice";
import TrueOrFalse from "./trueOrFalse";
import TypeAnswer from "./typeAnswer";
import { useConnectionId } from "@/app/util/store";
import user from "@/public/icons/user.png";
import Image from "next/image";

export default function Question() {
  const { connection } = useConnection();
  const [question, setQuestion] = useState();
  const [isShowLeader, setIsShowLeader] = useState();
  const [leaderBoard, setLeaderBoard] = useState();
  const { connectionId } = useConnectionId();

  console.log(connectionId);
  useEffect(() => {
    connection.on("question", (question) => {
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
      setQuestion(question);
      connection.on("leaderboard", (leader) => {
        setIsShowLeader(true);
        setLeaderBoard(() => leader);
        console.log(leader);
        setTimeout(() => {
          setIsShowLeader(false);
        }, 5000);
      });
    });
  }, []);
  return (
    <div className="flex flex-col h-full w-full items-center space-y-5  flex-grow ">
      {/* {question?.question.qTypeId === 1 && (
        <MulitpleChoice question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 3 && (
        <TrueOrFalse question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 4 && (
        <TypeAnswer question={question} connectionId={connectionId} />
      )} */}
      {/* <MulitpleChoice question={question} /> */}
      {/* <MultipleChoiceAudio /> */}
      {/* <MulitpleChoice /> */}
      {/* <TrueOrFalse /> */}
      {/* <TypeAnswer /> */}
      {/* {question && (
        <>
          <div>{question.remainingTime}</div>
          <div>{question.question.qStatement}</div>{" "}
        </>
      )} */}
      <div className="h-full w-full flex justify-center items-center ">
        <div className="w-1/4 h-3/4 bg-white p-5 overflow-auto space-y-5 rounded-lg">
          <div className="flex flex-row justify-between">
            <div className="font-bold">Leaderboard</div>
            <div className="flex flex-row space-x-1">
              <Image src={user} width={25} height={20} />
              <div>20</div>
            </div>
          </div>
          <div className="space-y-3">
            <div className="bg-gold flex flex-row p-2 rounded-lg">
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
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
