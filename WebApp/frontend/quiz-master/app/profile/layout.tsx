"use client";
import Link from "next/link";
import { ChevronLeftIcon, UserIcon } from "@heroicons/react/24/outline";
import { UserInfo } from "@/lib/definitions";
import React ,{ useEffect, useState } from "react";
import { fetchLoginUser } from "@/lib/quizData";


export default function Layout({ children }: { children: React.ReactNode }) {
    const [userInfo, setUserInfo] = useState<UserInfo>();

    useEffect(() => {
        fetchLoginUser().then((res) => {
            if (res !== null && res !== undefined) {
                setUserInfo(res);
            }
        });
    }, []);

    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <div className="flex-none transition w-12 md:w-64 ">
                <div className="flex-none w-12 md:w-64 h-full transition-all duration-500">
                    <div
                        className="h-full border border-r-2"
                        style={{
                            transform: false ? "translateX(256px)" : "none",
                        }}
                    >
                        <Link
                           // href="/dashboard"
                           href={userInfo?.info?.roles?.includes('Administrator')? "/dashboard":"/home"}
                            className="flex p-4 text-[14px]"
                        >
                            <ChevronLeftIcon className="w-6" />
                            <p className="hidden p-4 text-[14px] md:block transition-all duration-500">
                                Back
                            </p>
                        </Link>
                        <div>
                            <p className="text-[22px] font-bold px-5 hidden md:block">
                                User profile management
                            </p>
                        </div>
                        <div className="p-1 md:p-5">
                            <div className="flex text-white bg-[#18A24C] h-[40px] justify-center items-center rounded-md">
                                <UserIcon className="w-6" />
                                <p className="hidden md:block">
                                    Account Details
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="flex-grow p-6 md:overflow-y-auto md:p-10">
                {children}
            </div>
        </div>
    );
}
