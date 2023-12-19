"use client";

import React from "react";
import { Progress } from "@mantine/core";
import { useState, useEffect } from "react";

export default function TimeProgress() {
  const [time, setTime] = useState(100);
  // useEffect(() => {
  //   setInterval(() => {
  //     if (time > 0) {
  //       setTime((prev) => prev - 1);
  //       console.log(time);
  //     }
  //   }, 1000);
  // }, []);
  return <Progress value={time} />;
}
