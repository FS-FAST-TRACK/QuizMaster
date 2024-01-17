"use client";

import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
import { Suspense } from "react";
import Loading from "./loading";
import ErrorContainer from "@/components/pages/ErrorContainer";

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <SideNav />
            <div className="grow overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Quiz Rooms</PageHeader>
                <ErrorContainer>
                    <Suspense fallback={<Loading />}>{children}</Suspense>
                </ErrorContainer>
            </div>
        </div>
    );
}
