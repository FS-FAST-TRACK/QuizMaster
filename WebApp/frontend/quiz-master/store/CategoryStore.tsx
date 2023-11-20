import { QuestionCategory } from "@/lib/definitions";
import { create } from "zustand";

interface ICategoriesStore {
    questionCategories: QuestionCategory[];
    setQuestionCategories: (fetchedCategories: QuestionCategory[]) => void;
}

export const useQuestionCategoriesStore = create<ICategoriesStore>((set) => ({
    questionCategories: [],
    setQuestionCategories: (fetchedCategories: QuestionCategory[]) =>
        set({
            questionCategories: fetchedCategories,
        }),
}));
