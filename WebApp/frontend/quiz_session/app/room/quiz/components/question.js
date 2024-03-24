"use client";

import React from "react";
import { useEffect, useState } from "react";
import { useAnswer, useConnection } from "@/app/util/store";
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
import useUserTokenData from "@/app/util/useUserTokenData";

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
  const { answer } = useAnswer();
  const { isAdmin } = useUserTokenData();

  const { push } = useRouter();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  const answersRef = React.createRef(null);

  const handleCloseLeaderboard = () => {
    setResetLeader();
    setIsShowLeader(false);
  };

  useEffect(() => {
    if (leader.length > 0) {
      if (isStop) {
        setIsFinished(true);
        setLeaderBoard(leader);
      } else {
        setLeaderBoard(leader);
        setIsShowLeader(true);
        // setTimeout(() => {
        //   setResetLeader();
        //   setIsShowLeader(false);
        // }, 10000);
      }
    }
  }, [question, leader, isStop]);

  if (isShowLeader && !isFinished) {
    return (
      <Interval
        leaderBoard={leaderBoard}
        handleCloseLeaderboard={handleCloseLeaderboard}
      />
    );
  } else if (isFinished) {
    return (
      <>
        <ReactConfetti width={width} height={height} />
        <Leaderboard leaderBoard={leaderBoard} />
        <div className="p-6 justify-between flex">
          <div></div> {/* Add an empty div to push the button to the right */}
          <Button
            onClick={() =>
              goBackToLoby(
                params,
                connection,
                push,
                setResetLeader,
                setStart,
                isAdmin
              )
            }
            className={"w-auto bg-[#FF6633] p-2"}
            color="yellow"
          >
            {isAdmin ? "Go Back to Dashboard" : "Leave Room"}
          </Button>
        </div>
      </>
    );
  }

  // TODO I have already scaffolded a function to handle the screenshot for every question
  // It has to be uploaded to the server and send it to the report. Refer to Jay or someone present in the team
  // Jay's reply: Done na idol ;)

  return (
    <div ref={answersRef}>
      <TimeProgress />
      <Header />
      <div className="flex flex-col h-full w-full items-center space-y-5  flex-grow  ">
        {question?.question.qTypeId === 1 && (
          <MulitpleChoice
            ref={answersRef}
            question={question}
            connectionId={connectionId}
            answer={answer}
          />
        )}
        {question?.question.qTypeId === 2 && (
          <MultipleChoiceAudio
            ref={answersRef}
            question={question}
            connectionId={connectionId}
            answer={answer}
          />
        )}
        {question?.question.qTypeId === 3 && (
          <TrueOrFalse
            ref={answersRef}
            question={question}
            connectionId={connectionId}
            answer={answer}
          />
        )}
        {question?.question.qTypeId === 4 && (
          <TypeAnswer
            ref={answersRef}
            question={question}
            connectionId={connectionId}
            answer={answer}
          />
        )}
        {question?.question.qTypeId === 5 && (
          <Slider
            ref={answersRef}
            question={question}
            connectionId={connectionId}
            answer={answer}
          />
        )}
        {question?.question.qTypeId === 6 && (
          <DragAndDrop
            ref={answersRef}
            question={question}
            connectionId={connectionId}
            answer={answer}
          />
        )}
      </div>
    </div>
  );
}
