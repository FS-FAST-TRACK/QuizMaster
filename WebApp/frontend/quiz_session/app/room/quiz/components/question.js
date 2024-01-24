"use client";

import React from "react";
import { useEffect, useState } from "react";
import { useConnection } from "@/app/util/store";
import MultipleChoiceAudio from "./multipleChoiceAudio";
import MulitpleChoice from "./mulitpleChoice";
import TrueOrFalse from "./trueOrFalse";
import TypeAnswer from "./typeAnswer";
import { useConnectionId } from "@/app/util/store";
import Leaderboard from "./leaderboard";
import DragAndDrop from "./dragAndDrop";
import Slider from "./slider";
import ReactConfetti from "react-confetti";
import useWindowSize from "react-use/lib/useWindowSize";
import { Button } from "@mantine/core";
import { useRouter, useSearchParams } from "next/navigation";
import Interval from "./interval";
import { useQuestion, useLeaderboard, useStart } from "@/app/util/store";
import { goBackToLoby } from "@/app/auth/util/handlers";
import TimeProgress from "./progress";
import Header from "./header";

export default function Question() {
  const { width, height } = useWindowSize();
  const { connection } = useConnection();
  const { question } = useQuestion();
  const { setStart } = useStart();
  const { leader, isStop, setResetLeader } = useLeaderboard();
  const [isShowLeader, setIsShowLeader] = useState(false);
  const [isFinished, setIsFinished] = useState(false);
  const [leaderBoard, setLeaderBoard] = useState([]);
  const { connectionId } = useConnectionId();

  const { push } = useRouter();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  useEffect(() => {
    if (leader.length > 0) {
      if (isStop) {
        setIsFinished(true);
        setLeaderBoard(leader);
      } else {
        setLeaderBoard(leader);
        setIsShowLeader(true);
        setTimeout(() => {
          setResetLeader();
          setIsShowLeader(false);
        }, 10000);
      }
    }
  }, [question, leader, isStop]);

  if (isShowLeader && !isFinished) {
    return <Interval leaderBoard={leaderBoard} />;
  } else if (isFinished) {
    return (
      <>
        <ReactConfetti width={width} height={height} />
        <Leaderboard leaderBoard={leaderBoard} />
        <Button
          onClick={() =>
            goBackToLoby(params, connection, push, setResetLeader, setStart)
          }
        >
          Go back to Loby
        </Button>
      </>
    );
  }
  return (
    <>
      <TimeProgress />
      <Header />
      <div className="flex flex-col h-full w-full items-center space-y-5  flex-grow  ">
        {question?.question.qTypeId === 1 && (
          <MulitpleChoice question={question} connectionId={connectionId} />
        )}
        {question?.question.qTypeId === 2 && (
          <MultipleChoiceAudio
            question={question}
            connectionId={connectionId}
          />
        )}
        {question?.question.qTypeId === 3 && (
          <TrueOrFalse question={question} connectionId={connectionId} />
        )}
        {question?.question.qTypeId === 4 && (
          <TypeAnswer question={question} connectionId={connectionId} />
        )}
        {question?.question.qTypeId === 5 && (
          <Slider question={question} connectionId={connectionId} />
        )}
        {question?.question.qTypeId === 6 && (
          <DragAndDrop question={question} connectionId={connectionId} />
        )}
      </div>
    </>
  );
}
