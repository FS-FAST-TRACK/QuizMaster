import { Question } from "@/lib/definitions";
import { create } from "zustand";

interface IQuestionStore {
    questionnaire: Question;
    setQuestionnaire: (fetchedQuestionnaire: Question) => void;
    getLatestQuestionnaire: () => Question | undefined;
}

export const QUESTION_DEFAULT = {
    id: -1,
    qAudio: "",
    qCategoryId: 1,
    qDifficultyId: 1,
    qTypeId: 1,
    qImage: "",
    qStatement: "",
    qTime: 30,
    details: [],
} as Question;

export const useQuestionnaire = create<IQuestionStore>((set, get) => ({
    questionnaire: {
        id: -1,
        qAudio: "",
        qCategoryId: 1,
        qDifficultyId: 1,
        qTypeId: 1,
        qImage: "",
        qStatement: "",
        qTime: 30,
        details: [],
    } as Question,
    setQuestionnaire(fetchedQuestionnaire) {
        set({ questionnaire: fetchedQuestionnaire });
    },
    getLatestQuestionnaire() {
        return get().questionnaire;
    },
}));
