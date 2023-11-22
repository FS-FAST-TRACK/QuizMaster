import { QuestionDifficulty } from "@/lib/definitions";
import { create } from "zustand";

interface IDifficultiesStore {
    questionDifficulties: QuestionDifficulty[];
    setQuestionDifficulties: (
        fetchedDifficulties: QuestionDifficulty[]
    ) => void;
    getQuestionDifficultyDescription: (id: number) => string | undefined;
}

export const useQuestionDifficultiesStore = create<IDifficultiesStore>(
    (set, get) => ({
        questionDifficulties: [],
        setQuestionDifficulties: (fetchedDifficulties: QuestionDifficulty[]) =>
            set({
                questionDifficulties: fetchedDifficulties,
            }),
        getQuestionDifficultyDescription(id) {
            return get().questionDifficulties.find((dif) => dif.id === id)
                ?.qDifficultyDesc;
        },
    })
);
