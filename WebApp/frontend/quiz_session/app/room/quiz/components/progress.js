"use client";

import React from "react";
import { Progress } from "@mantine/core";
import { useState, useEffect } from "react";
import { useConnection, useQuestion } from "@/app/util/store";

const SFX_TRIGGER_SECONDS =
  process.env.QUIZMASTER_TRIGGER_SFX_SECONDS ??
  process.env.NEXT_PUBLIC_QUIZMASTER_TRIGGER_SFX_SECONDS;

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
    <Progress
      value={time}
      color={question?.remainingTime > SFX_TRIGGER_SECONDS ? "white" : "red"}
      bg={"green"}
      transitionDuration={1000}
    />
  );
}
