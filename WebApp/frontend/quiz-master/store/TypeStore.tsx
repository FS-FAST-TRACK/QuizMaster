import { QuestionType } from "@/lib/definitions";
import { create } from "zustand";

interface ITypesStore {
    questionTypes: QuestionType[];
    setQuestionTypes: (fetchedTypes: QuestionType[]) => void;
    getQuestionTypeDescription: (id: number) => string | undefined;
}

// Jayharron added this since it's a request from the management: 2/27/2024
// Only using Multiple Choice, True or False and Type Answer for now..., remove this
// filter if you need to use all question types.
const filter = (element: QuestionType) => {
    return element.id === 1 || element.id === 3 || element.id === 4;
};

export const useQuestionTypesStore = create<ITypesStore>((set, get) => ({
    questionTypes: [],
    setQuestionTypes: (fetchedTypes: QuestionType[]) =>
        set({
            questionTypes: fetchedTypes.filter(filter),
        }),
    getQuestionTypeDescription(id) {
        return get().questionTypes.find((type) => type.id === id)?.qTypeDesc;
    },
}));
