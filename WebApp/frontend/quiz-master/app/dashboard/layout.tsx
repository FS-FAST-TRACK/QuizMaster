"use client"
import SideNav from "@/components/Commons/navbars/sidenav";
import PageHeaderDashboard from "@/components/Commons/headers/PageHeaderDashboard";
import Card from "./card";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
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
        <div className="flex h-screen flex-row md:overflow-hidden ">
            <div className="flex-none transition w-12 md:w-64 ">
                <SideNav />
                
            </div>
            <div className="w-full  h-screen flex flex-col  relative">
            <PageHeaderDashboard>Dashboard</PageHeaderDashboard>
            <Card />
            </div>
        </div>
    );
}
