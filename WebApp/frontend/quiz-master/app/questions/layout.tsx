"use client";

import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
import { fetchCategories, fetchDifficulties, fetchTypes } from "@/lib/quizData";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { useEffect } from "react";

export default function Layout({ children }: { children: React.ReactNode }) {
    const { setQuestionCategories } = useQuestionCategoriesStore();
    const { setQuestionDifficulties } = useQuestionDifficultiesStore();
    const { setQuestionTypes } = useQuestionTypesStore();

    useEffect(() => {
        fetchCategories().then((res) => {
            setQuestionCategories(res.data);
        });
        fetchDifficulties().then((res) => {
            setQuestionDifficulties(res);
        });
        fetchTypes().then((res) => {
            setQuestionTypes(res);
        });
    }, [setQuestionCategories, setQuestionDifficulties, setQuestionTypes]);

    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <div className="flex-none transition w-64 hidden md:block">
                <SideNav />
            </div>
            <div className="grow md:overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Questions</PageHeader>
                {children}
            </div>
        </div>
    );
}
