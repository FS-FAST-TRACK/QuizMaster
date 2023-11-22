import { QuestionCategory } from "@/lib/definitions";
import { create } from "zustand";

interface ICategoriesStore {
    questionCategories: QuestionCategory[];
    setQuestionCategories: (fetchedCategories: QuestionCategory[]) => void;
    getQuestionCategoryDescription: (id: number) => string | undefined;
}

export const useQuestionCategoriesStore = create<ICategoriesStore>(
    (set, get) => ({
        questionCategories: [],
        setQuestionCategories: (fetchedCategories: QuestionCategory[]) =>
            set({
                questionCategories: fetchedCategories,
            }),
        getQuestionCategoryDescription: (id: number) => {
            const state = get();
            return state.questionCategories.find((cat) => cat.id === id)
                ?.qCategoryDesc;
        },
    })
);
