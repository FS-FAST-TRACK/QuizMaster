"use client";

import { QuestionCreateValues, QuestionValues } from "@/lib/definitions";
import { UseFormReturnType } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";
import MultipleChoicePlusAudioQuestionDetail from "./MultipleChoicePlusAudio";
import TrueOrFalseQuestionDetail from "./TrueOrFalse";
import TypeAnswerQuestionDetails from "./TypeAnwer";
import SliderQuestionDetails from "./Slider";
import PuzzleQuestionDetails from "./Puzzle";

export default function QuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionValues>;
}) {
    if (form.values.qTypeId === "1") {
        return <MultipleChoiceQuestionDetail form={form} />;
    }
    if (form.values.qTypeId === "2") {
        return <MultipleChoicePlusAudioQuestionDetail form={form} />;
    }
    if (form.values.qTypeId === "3") {
        return <TrueOrFalseQuestionDetail form={form} />;
    }

    if (form.values.qTypeId === "4") {
        return <TypeAnswerQuestionDetails form={form} />;
    }

    if (form.values.qTypeId === "5") {
        return <SliderQuestionDetails form={form} />;
    }

    if (form.values.qTypeId === "6") {
        return <PuzzleQuestionDetails form={form} />;
    }

    return <div></div>;
}
