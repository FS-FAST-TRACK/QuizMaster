import React from "react";
import TimeProgress from "./components/progress";
import Question from "./components/question";
import Header from "./components/header";

export default function page() {
  return (
    <div className="h-full bg-red-500 flex flex-col">
      <TimeProgress />
      <Header />

      <Question />
    </div>
  );
}
