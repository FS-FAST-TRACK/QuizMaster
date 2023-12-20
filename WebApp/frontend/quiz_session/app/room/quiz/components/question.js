"use client";

import React from "react";
import { useEffect, useState } from "react";
import { useConnection } from "@/app/util/store";
import MultipleChoiceAudio from "./multipleChoiceImage";
import MulitpleChoice from "./mulitpleChoice";
import TrueOrFalse from "./trueOrFalse";
import TypeAnswer from "./typeAnswer";

export default function Question() {
  const { connection } = useConnection();
  const [question, setQuestion] = useState();

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
  }, []);
  return (
    <div className="flex flex-col w-full items-center space-y-5 pt-5 flex-grow bg-yellow-500">
      {/* {question?.question.qTypeId === 1 && (
        <MulitpleChoice question={question} />
      )}
      {question?.question.qTypeId === 3 && <TrueOrFalse question={question} />}
      {question?.question.qTypeId === 4 && <TypeAnswer question={question} />} */}
      <MulitpleChoice question={question} />
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
