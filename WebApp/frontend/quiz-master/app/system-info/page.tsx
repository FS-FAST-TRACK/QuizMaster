import { useEffect, useState } from "react";
import quizMasterIcon from "/public/quiz-master-icon-white.svg";
import Image from "next/image";
import { SystemInfoDto } from "@/lib/definitions";
import { fetchSystemInfo } from "@/lib/quizData";
import SystemInfoCard from "@/components/Commons/cards/SystemInfoCard";
import { getServerSession } from "next-auth";

export default async function SystemInfo() {
    const session = await getServerSession();

    return (
        <div className="flex flex-row gap-5">
            <SystemInfoCard email={`${session?.user.email}`} />
            <div className="hidden sm:block">
                <Image
                    src={quizMasterIcon}
                    alt="QuizMaster Icon"
                    width={1000}
                    height={1000}
                />
            </div>
        </div>
    );
}
