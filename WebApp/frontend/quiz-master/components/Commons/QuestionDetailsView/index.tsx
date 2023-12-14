"use client";

import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionValues,
} from "@/lib/definitions";
import { UseFormReturnType } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";
import MultipleChoicePlusAudioQuestionDetail from "./MultipleChoicePlusAudio";
import TrueOrFalseQuestionDetail from "./TrueOrFalse";
import TypeAnswerQuestionDetails from "./TypeAnwer";
import SliderQuestionDetails from "./Slider";
import PuzzleQuestionDetails from "./Puzzle";
import { useEffect, useState } from "react";
import { getQuestionDetails } from "@/lib/hooks/questionDetails";

export default function QuestionDetails({
    questionId,
    questionTypeId,
}: {
    questionId?: number;
    questionTypeId?: number;
}) {
    const [questionDetails, setQuestionDetails] = useState<QuestionDetail[]>();
    useEffect(() => {
        if (questionId)
            getQuestionDetails({ questionId }).then((res) => {
                setQuestionDetails(res);
            });
    }, [questionId]);

    if (questionTypeId === 1) {
        return <MultipleChoiceQuestionDetail details={questionDetails} />;
    }
    if (questionTypeId === 2) {
        return (
            <MultipleChoicePlusAudioQuestionDetail details={questionDetails} />
        );
    }
    if (questionTypeId === 3) {
        return <TrueOrFalseQuestionDetail details={questionDetails} />;
    }

    if (questionTypeId === 4) {
        return <TypeAnswerQuestionDetails details={questionDetails} />;
    }

    if (questionTypeId === 5) {
        return <SliderQuestionDetails details={questionDetails} />;
    }

    if (questionTypeId === 6) {
        return <PuzzleQuestionDetails details={questionDetails} />;
    }

    return <div></div>;
}
