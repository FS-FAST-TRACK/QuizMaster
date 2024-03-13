"use client";

import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { Suspense, useCallback, useEffect } from "react";
import Loading from "./loading";
import { getAllCategories } from "@/lib/hooks/category";
import { getAllDifficulties } from "@/lib/hooks/difficulty";
import { getAllTypes } from "@/lib/hooks/type";
import ErrorContainer from "@/components/pages/ErrorContainer";
import { useErrorRedirection } from "@/utils/errorRedirection";
import { fetchLoginUser } from "@/lib/quizData";
import { useRouter } from "next/navigation";
import { useQuestionnaire } from "@/store/QuestionStore";
import { GetAllQuestion } from "@/lib/hooks/question";

export default function Layout({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    useEffect(() => {
        fetchLoginUser().then((res) => {
            if (res !== null && res !== undefined) {
                if (!res?.info.roles.includes("Administrator")) {
                    router.push("/home");
                }
            }
        });
    }, []);

    const { setQuestionCategories } = useQuestionCategoriesStore();
    const { setQuestionDifficulties } = useQuestionDifficultiesStore();
    const { setQuestionTypes } = useQuestionTypesStore();
    const { setQuestionnaire } = useQuestionnaire();
    const { redirectToError } = useErrorRedirection();

    const populateData = useCallback(async () => {
        try {
            const categoriesRes = await getAllCategories();
            setQuestionCategories(categoriesRes.data);

            const difficultiesRes = await getAllDifficulties();
            setQuestionDifficulties(difficultiesRes.data);

            const typesRes = await getAllTypes();
            setQuestionTypes(typesRes.data);

            const questionsRes = await GetAllQuestion();
            if (questionsRes) setQuestionnaire(questionsRes.pop());
        } catch (error) {
            redirectToError();
        }
    }, []);

    useEffect(() => {
        populateData();
    }, [setQuestionCategories, setQuestionDifficulties, setQuestionTypes]);

    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <SideNav />
            <div className="grow overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Questions</PageHeader>
                <ErrorContainer>
                    <Suspense fallback={<Loading />}>{children}</Suspense>
                </ErrorContainer>
            </div>
        </div>
    );
}
