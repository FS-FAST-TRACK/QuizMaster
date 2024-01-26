"use client";

import Link from "next/link";
import Image from "next/image";
import logo from "/public/quiz-master-logo-white.png";
import userIcon from "/public/user-icon.svg";
import { useEffect, useState } from "react";
import { fetchLoginUser } from "@/lib/quizData";
import { UserInfo } from "@/lib/definitions";
import { Divider } from "@mantine/core";

export default function HeadNav() {
    const [userInfo, setUserInfo] = useState<UserInfo>();

    useEffect(() => {
        fetchLoginUser().then((res) => {
            if (res !== null && res !== undefined) {
                setUserInfo(res);
            }
        });
    }, []);

    return (
        <div className="flex flex-col gap-4">
            <div className="flex flex-row w-full gap-10 h-10 text-white transition-all duration-500">
                <div className="flex flex-row rounded-3xl items-center gap-10">
                    <Link href="/dashboard" className="hidden lg:block">
                        <div className="text-white">
                            <Image
                                src={logo}
                                alt="QuizMaster Logo"
                                width={100}
                                height={100}
                                priority
                            />
                        </div>
                    </Link>
                    <Link href="/">Home</Link>
                    <Link href="/system-info">About</Link>
                    <Link href="/contact-us">Contact Us</Link>
                </div>
                <div className="flex grow flex-row justify-between space-x-0"></div>
                {userInfo ? (
                    <div className="flex flex-row justify-center items-center w-32 p-2 bg-[#18A44C] gap-2 rounded-md hover:bg-[#00E154] hover:cursor-pointer">
                        <Image
                            src={userIcon}
                            alt="QuizMaster Logo"
                            width={20}
                            height={20}
                            priority
                        />
                        <p>{userInfo?.info?.userData?.userName}</p>
                    </div>
                ) : (
                    <div className="flex flex-row rounded-3xl items-center gap-10">
                        <Link href="/auth/login">Login</Link>
                        <div className="bg-[#FF7F2A] p-2 rounded">
                            <Link href="/auth/signup">Sign Up</Link>
                        </div>
                    </div>
                )}
            </div>
            <Divider />
        </div>
    );
}
