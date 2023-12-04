"use client";

import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
import { fetchCategories, fetchDifficulties, fetchTypes } from "@/lib/quizData";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { Suspense, useEffect } from "react";
import Loading from "./loading";

export default function Layout({ children }: { children: React.ReactNode }) {
    const { setQuestionCategories } = useQuestionCategoriesStore();
    const { setQuestionDifficulties } = useQuestionDifficultiesStore();
    const { setQuestionTypes } = useQuestionTypesStore();

    useEffect(() => {
        fetchCategories().then((res) => {
            setQuestionCategories(res.data);
        });
        fetchDifficulties().then((res) => {
            setQuestionDifficulties(res.data);
        });
        fetchTypes().then((res) => {
            setQuestionTypes(res);
        });
    }, [setQuestionCategories, setQuestionDifficulties, setQuestionTypes]);

    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <SideNav />
            <div className="grow overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Questions</PageHeader>
                <Suspense fallback={<Loading />}>{children}</Suspense>
            </div>
        </div>
    );
}
