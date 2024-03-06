"use client"
import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
import { fetchLoginUser } from "@/lib/quizData";
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
    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <SideNav />
            <div className="grow md:overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Difficulties</PageHeader>
                {children}
            </div>
        </div>
    );
}
