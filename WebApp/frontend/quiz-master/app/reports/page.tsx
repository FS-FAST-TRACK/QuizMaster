"use client";

import QuestionReport from "./components/questionsReport";

export default function Page() {
    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <QuestionReport />
        </div>
    );
}
