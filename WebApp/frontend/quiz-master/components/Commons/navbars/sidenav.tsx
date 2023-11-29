"use client";

import Link from "next/link";
import NavLinks from "@/components/Commons/navbars/nav-links";
import { PowerIcon } from "@heroicons/react/24/outline";
import Image from "next/image";
import logo from "/public/quiz-master-logo.png";
import UserNavBar from "./UserNavBar";
import { useState } from "react";

export default function SideNav() {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className="flex-none w-12 md:w-64 h-full transition-all duration-500">
            <div
                className="h-full border border-r-2"
                style={{
                    transform: isOpen ? "translateX(256px)" : "none",
                }}
            >
                <div className="flex h-full flex-col rounded-3xl">
                    <Link
                        className="flex items-end justify-center rounded-md p-8 mb-16"
                        href="/dashboard"
                    >
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
                    <div className="flex grow flex-col justify-between space-x-0">
                        <NavLinks />
                        <div className="h-auto w-full grow rounded-md bg-transparent block"></div>
                        <UserNavBar userName="Admin" email="admin@gmail.com" />
                    </div>
                </div>
            </div>
        </div>
    );
}
