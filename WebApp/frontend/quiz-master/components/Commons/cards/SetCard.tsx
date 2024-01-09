import { Question, QuestionSet, Set } from "@/lib/definitions";
import { fetchQuestion, fetchSetQuestions } from "@/lib/quizData";
import { Button } from "@mantine/core";
import { useEffect, useState } from "react";
import QuestionTable from "../tables/QuestionTable";
import { useDisclosure } from "@mantine/hooks";

export default function SetCard({ set }: { set?: Set }) {
    const [questionsInSet, setQuestionsInSet] = useState<QuestionSet[]>([]);
    const [questions, setQuestions] = useState<Question[]>([]);
    const [visible, { close, open }] = useDisclosure(false);
    const [count, setCount] = useState<number>(0);

    useEffect(() => {
        if (set) {
            fetchSetQuestions({ setId: set.id }).then((res) => {
                setQuestionsInSet(res);
            });
        }
    }, []);

    useEffect(() => {
        if (questionsInSet.length > 0 && count < 1) {
            setCount(count + 1);
            questionsInSet.map((qSet) => {
                fetchQuestion({ questionId: qSet.questionId }).then((r) => {
                    setQuestions((prev) => [...prev, r.data]);
                });
            });
        }
    }, [questionsInSet]);

    if (!set) {
        return;
    }
    return (
        <div className="space-y-8 p-3">
            <div className="flex justify-between">
                <div>
                    <p>Set Name</p>
                    <p className="text-xl font-bold">{set && set.qSetName}</p>
                </div>
                <div>
                    <p>No. of Questions</p>
                    <p className="text-xl font-bold">
                        {questions && questions.length}
                    </p>
                </div>
            </div>
            <div>
                <p>Questions</p>
                <QuestionTable
                    questions={questions}
                    message={
                        questions.length === 0 ? "No Questions" : undefined
                    }
                    setSelectedRow={() => null}
                    loading={visible}
                    callInQuestionsPage="set single view"
                />
            </div>
        </div>
    );
}
