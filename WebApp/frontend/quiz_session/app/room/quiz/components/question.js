"use client";

import React from "react";
import { useEffect, useState } from "react";
import { useConnection } from "@/app/util/store";
import MultipleChoiceAudio from "./multipleChoiceAudio";
import MultipleChoiceImage from "./multipleChoiceImage";
import MulitpleChoice from "./mulitpleChoice";
import TrueOrFalse from "./trueOrFalse";
import TypeAnswer from "./typeAnswer";
import { useConnectionId } from "@/app/util/store";
import user from "@/public/icons/user.png";
import Image from "next/image";
import Leaderboard from "./leaderboard";
import DragAndDrop from "./dragAndDrop";
import dynamic from "next/dynamic";
import Slider from "./slider";
import ReactConfetti from "react-confetti";
import useWindowSize from "react-use/lib/useWindowSize";
import { Button } from "@mantine/core";
import { useRouter, useSearchParams } from "next/navigation";
import Interval from "./interval";
import { notifications } from "@mantine/notifications";

export default function Question() {
  const { width, height } = useWindowSize();
  const { connection } = useConnection();
  const [question, setQuestion] = useState();
  const [isShowLeader, setIsShowLeader] = useState(false);
  const [isFinished, setIsFinished] = useState(false);
  const [leaderBoard, setLeaderBoard] = useState([]);
  const { connectionId } = useConnectionId();

  const { push, back } = useRouter();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

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

    connection.on("leaderboard", (leader, isStop) => {
      if (isStop) {
        setIsFinished(true);
      } else {
        setLeaderBoard(leader);
        setIsShowLeader(true);
        setTimeout(() => {
          setIsShowLeader(false);
        }, 10000);
      }
    });

    // connection.on("notif", (message) => {
    //   console.log("notification from Question");
    //   notifications.show({
    //     title: message + "notification from Question",
    //   });
    // });
  }, []);

  const goBackToLoby = () => {
    try {
      const code = params.get("roomPin");
      connection.invoke("GetRoomParticipants", code);
      back();
    } catch (ex) {
      console.log(ex);
    }
  };

  if (isShowLeader && !isFinished) {
    return <Interval leaderBoard={leaderBoard} />;
  } else if (isFinished) {
    return (
      <>
        <ReactConfetti width={width} height={height} />
        <Leaderboard leaderBoard={leaderBoard} />
        <Button onClick={goBackToLoby}>Go back to Loby</Button>
      </>
    );
  }
  return (
    <div className="flex flex-col h-full w-full items-center space-y-5  flex-grow ">
      {question?.question.qTypeId === 1 && (
        <MulitpleChoice question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 3 && (
        <TrueOrFalse question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 4 && (
        <TypeAnswer question={question} connectionId={connectionId} />
      )}
      {question?.question.qTypeId === 5 && <Slider />}
      {question?.question.qTypeId === 6 && <DragAndDrop />}
      {/* {isShowLeader && <Leaderboard leaderBoard={leaderBoard} />}
      {isFinished && <Leaderboard leaderBoard={leaderBoard} />} */}
      {/* <MulitpleChoice question={question} /> */}
      {/* <MultipleChoiceImage /> */}
      {/* <MulitpleChoice /> */}
      {/* <TrueOrFalse /> */}
      {/* <TypeAnswer /> */}
      {/* {question && (
        <>
          <div>{question.remainingTime}</div>
          <div>{question.question.qStatement}</div>{" "}
        </>
      )} */}
      {/* <DynamicDragAndDrop /> */}
      {/* <MultipleChoiceAudio /> */}
      {/* <Slider /> */}
    </div>
  );
}
