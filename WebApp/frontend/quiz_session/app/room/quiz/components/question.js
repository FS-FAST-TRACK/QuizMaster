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
import Leaderboard from "./leaderboard";

export default function Question() {
  const { connection } = useConnection();
  const [question, setQuestion] = useState();
  const [isShowLeader, setIsShowLeader] = useState();
  const [leaderBoard, setLeaderBoard] = useState([]);
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
    });

    connection.on("leaderboard", (leader) => {
      setIsShowLeader(true);
      setLeaderBoard(leader);
      setTimeout(() => {
        setIsShowLeader(false);
      }, 5000);
    });
  }, []);
  return (
    <div className="flex flex-col h-full w-full items-center space-y-5  flex-grow ">
      {question?.question.qTypeId === 1 && !isShowLeader && (
        <MulitpleChoice question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 3 && !isShowLeader && (
        <TrueOrFalse question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 4 && !isShowLeader && (
        <TypeAnswer question={question} connectionId={connectionId} />
      )}
      {isShowLeader && <Leaderboard leaderBoard={leaderBoard} />}
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
    </div>
  );
}
