"use client";

import QuestionTable from "@/components/Commons/tables/QuestionTable";
import { fetchQuestions } from "@/lib/quizData";
import { useEffect } from "react";

export default function Page() {
  return (
    <div className="flex flex-col">
      <div className="flex flex-col md:flex-row justify-between ">
        <h3>Questions</h3>
      </div>
      <QuestionTable />
    </div>
  );
}
