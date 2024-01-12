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
import { useCallback, useEffect, useState } from "react";
import {
    fetchQuestionDetails,
    getQuestionDetails,
} from "@/lib/hooks/questionDetails";
import { PuzzleData } from "@/lib/questionTypeData";
import { notification } from "@/lib/notifications";
import { useErrorRedirection } from "@/utils/errorRedirection";

export default function QuestionDetails({
    questionId,
    questionTypeId,
}: {
    questionId?: number;
    questionTypeId?: number;
}) {
    const [questionDetails, setQuestionDetails] = useState<QuestionDetail[]>();
    const { redirectToError } = useErrorRedirection();

    const populateDetails = useCallback(async () => {
        try {
            const res = await fetchQuestionDetails({ questionId: questionId! });
            // filter out the question details
            var filteredDetails: QuestionDetail[] = res.data;

            // special block of code to handle options positioning
            if (questionTypeId && questionTypeId === PuzzleData.id) {
                try {
                    // get the answer detail for puzzle questions
                    var answerDetail = filteredDetails.find((q) =>
                        q.detailTypes.includes("answer")
                    );
                    var answerDescription = answerDetail!.qDetailDesc;

                    const answer: number[] = JSON.parse(
                        answerDescription[0] !== "["
                            ? `[${answerDescription}]`
                            : answerDescription
                    );
                    var filteredDetails = answer.map(
                        (id) => filteredDetails.find((d) => d.id === id)!
                    );
                } catch (error) {
                    notification({
                        type: "error",
                        title: "This puzzle queston doesn't have an answer.",
                    });
                    throw new Error("No answer found");
                }
            }
            setQuestionDetails(filteredDetails);
        } catch (error) {
            redirectToError();
        }
    }, [questionId, questionTypeId]);
    useEffect(() => {
        if (questionId) populateDetails();
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
