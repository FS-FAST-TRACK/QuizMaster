import React from "react";
import TimeProgress from "./components/progress";
import Question from "./components/question";
import Header from "./components/header";

export default function page() {
  return (
    <div className="h-full  flex flex-col">
      <Question />
    </div>
  );
}
