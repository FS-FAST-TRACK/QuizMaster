import { QuestionType } from "@/lib/definitions";
import { create } from "zustand";

interface ITypesStore {
    questionTypes: QuestionType[];
    setQuestionTypes: (fetchedTypes: QuestionType[]) => void;
    getQuestionTypeDescription: (id: number) => string | undefined;
}

export const useQuestionTypesStore = create<ITypesStore>((set, get) => ({
    questionTypes: [],
    setQuestionTypes: (fetchedTypes: QuestionType[]) =>
        set({
            questionTypes: fetchedTypes,
        }),
    getQuestionTypeDescription(id) {
        return get().questionTypes.find((type) => type.id === id)?.qTypeDesc;
    },
}));
