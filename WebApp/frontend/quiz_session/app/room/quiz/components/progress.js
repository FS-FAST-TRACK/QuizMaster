"use client";

import React from "react";
import { Progress } from "@mantine/core";
import { useState, useEffect } from "react";
import { useConnection, useQuestion } from "@/app/util/store";

export default function TimeProgress() {
  const { connection } = useConnection();
  const { question } = useQuestion();
  const [time, setTime] = useState();
  useEffect(() => {
    if (question !== undefined) {
      var percent = Math.floor(
        (question.remainingTime / question.question.qTime) * 100
      );
      setTime(percent);
    }
  }, [question]);
  return (
    <Progress value={time} color="white" bg="green" transitionDuration={1000} />
  );
}
