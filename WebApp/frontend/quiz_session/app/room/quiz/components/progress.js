"use client";

import React from "react";
import { Progress } from "@mantine/core";
import { useState, useEffect } from "react";
import { useConnection } from "@/app/util/store";

export default function TimeProgress() {
  const { connection } = useConnection();
  const [time, setTime] = useState();
  useEffect(() => {
    connection.on("question", (question) => {
      console.log(question);
      /*
       question - question.question.qStatement
       answers - question.details.qDetailDesc
       type - question.question.qTypeId
       question duration - question.question.qTime
       remaining time - question.remainingTime

       Type Questions
       1 - Multiple Choice
       2 - Multiple Choice + Audio
       3 - True or False
       4 - Type Answer
       5 - Slider
       6 - Puzzle
      */
      var percent = Math.floor(
        (question.remainingTime / question.question.qTime) * 100
      );
      setTime(percent);
      console.log(question.remainingTime);
    });
  }, []);
  return <Progress value={time} />;
}
