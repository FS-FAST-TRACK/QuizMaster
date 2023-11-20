import { QuestionType } from "@/lib/definitions";
import { create } from "zustand";

interface ITypesStore {
    questionTypes: QuestionType[];
    setQuestionTypes: (fetchedTypes: QuestionType[]) => void;
}

export const useQuestionTypesStore = create<ITypesStore>((set) => ({
    questionTypes: [],
    setQuestionTypes: (fetchedTypes: QuestionType[]) =>
        set({
            questionTypes: fetchedTypes,
        }),
}));
