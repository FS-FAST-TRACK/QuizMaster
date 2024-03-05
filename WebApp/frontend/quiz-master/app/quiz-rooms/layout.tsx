"use client";

import SideNav from "@/components/Commons/navbars/sidenav";
import { Suspense, useEffect } from "react";
import Loading from "./loading";
import ErrorContainer from "@/components/pages/ErrorContainer";
import PageHeader from "@/components/Commons/headers/PageHeaderQuizRoom";
import { useRouter } from "next/navigation";
import { fetchLoginUser } from "@/lib/quizData";
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
   
    return (
        <div className="flex h-screen flex-row md:overflow-hidden" style={{overflow:'auto',overflowY:'auto'}} >
            <SideNav />
            <div className="grow overflow-y-auto flex flex-col bg-[#F8F9FA]">
               <PageHeader>Quiz Rooms </PageHeader>
               
                <ErrorContainer>
                    <Suspense fallback={<Loading />}>{children}</Suspense>
                </ErrorContainer>
            </div>
        </div>
    );
}
