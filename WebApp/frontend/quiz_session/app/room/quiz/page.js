import React from "react";
import TimeProgress from "./components/progress";
import Question from "./components/question";
import Header from "./components/header";

export default function page() {
  return (
    <div>
      <TimeProgress />
      <Header />

      <Question />
    </div>
  );
}
