"use client";

import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
import { getAllCategories } from "@/lib/hooks/category";
import { getAllDifficulties } from "@/lib/hooks/difficulty";
import { getAllTypes } from "@/lib/hooks/type";
import { fetchLoginUser } from "@/lib/quizData";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function Layout({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    useEffect(()=>{
        fetchLoginUser().then((res) => {
            if (res !== null && res !== undefined) {
                if(!(res?.info.roles.includes("Administrator"))){
                    router.push("/home");
                }
            }
        });
    }, [])
    const { setQuestionCategories } = useQuestionCategoriesStore();
    const { setQuestionDifficulties } = useQuestionDifficultiesStore();
    const { setQuestionTypes } = useQuestionTypesStore();

    useEffect(() => {
        getAllCategories().then((res) => {
            setQuestionCategories(res.data);
        });
        getAllDifficulties().then((res) => {
            setQuestionDifficulties(res.data);
        });
        getAllTypes().then((res) => {
            setQuestionTypes(res.data);
        });
    }, [setQuestionCategories, setQuestionDifficulties, setQuestionTypes]);

    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <SideNav />
            <div className="grow md:overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Question Sets</PageHeader>
                {children}
            </div>
        </div>
    );
}
