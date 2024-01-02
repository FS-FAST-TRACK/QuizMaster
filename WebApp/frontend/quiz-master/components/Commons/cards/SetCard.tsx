import { Question, QuestionSet, Set } from "@/lib/definitions";
import { fetchSetQuestions } from "@/lib/quizData";
import { useEffect, useState } from "react";

export default function QuesitonCard({ set }: { set?: Set }) {
    const [questionsInSet, setQuestionsInSet] = useState<QuestionSet[]>([]);

    useEffect(() => {
        if (set) {
            fetchSetQuestions({ setId: set.id }).then((res) => {
                console.log(res);
                setQuestionsInSet(res);
            });
        }
    }, []);

    if (!set) {
        return;
    }
    return (
        <div className="space-y-8">
            <div className="flex items-center">
                <span className="text-3xl font-bold mr-1">&#8226;</span>
                <p
                    className="text-xl font-bold mr-1"
                    style={{ listStyle: "none" }}
                >
                    {set?.qSetName}
                </p>
                <p className="text-base font-base">{` (${questionsInSet.length} questions)`}</p>
            </div>
        </div>
    );
}
