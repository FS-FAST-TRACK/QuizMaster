import { QuestionDifficulty } from "@/lib/definitions";
import { create } from "zustand";

interface IDifficultiesStore {
    questionDifficulties: QuestionDifficulty[];
    setQuestionDifficulties: (
        fetchedDifficulties: QuestionDifficulty[]
    ) => void;
}

export const useQuestionDifficultiesStore = create<IDifficultiesStore>(
    (set) => ({
        questionDifficulties: [],
        setQuestionDifficulties: (fetchedDifficulties: QuestionDifficulty[]) =>
            set({
                questionDifficulties: fetchedDifficulties,
            }),
    })
);
