"use client";

import Link from "next/link";
import Image from "next/image";
import logo from "/public/quiz-master-logo-white.png";
import userIcon from "/public/user-icon.svg";
import { useEffect, useState } from "react";
import { fetchLoginUser } from "@/lib/quizData";
import { UserInfo } from "@/lib/definitions";
import { Divider } from "@mantine/core";

import {
    ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/outline";
export default   function HeadNav() {
    const [loading,setLoading]= useState(false);
    const [userInfo, setUserInfo] = useState<UserInfo>();
    useEffect(() => {
        fetchLoginUser().then((res) => {
            setLoading(false);
            if (res !== null && res !== undefined) {
                setUserInfo(res);
            }
            setTimeout(()=>{
                setLoading(true);
            },100)
        });
    }, []);

    return (
        <div className="flex flex-col gap-4">
            <div className="flex flex-row w-full gap-10 h-10 text-white transition-all duration-500">
                <div className="flex flex-row rounded-3xl items-center gap-10">
                    <Link href={userInfo?.info.roles.includes("Administrator")? "/dashboard":"/"} className="hidden lg:block">
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
                    <Link href="/"><b>Home</b></Link>
                    <Link href="/system-info"><b>About</b></Link>
                    <Link  href="/contact-us"><b>Contact Us</b></Link>
                </div>
                <div  className="flex grow flex-row justify-between space-x-0"></div>
              
              <div style={{display:loading?"flex":"none"}}>
              {userInfo ? (
                 <div className="flex justify-between">
                 <Link
                   href="/profile"
                  className="flex flex-row justify-center items-center w-32 p-5 bg-[#18A44C] gap-2 rounded-md hover:bg-[#00E154] hover:cursor-pointer">
                   <Image src={userIcon} alt="profile icon" width={20} height={20} priority />
                   <p>{userInfo?.info?.userData?.userName}</p>
                 </Link>
                 <Link href="/auth/signout"  className="flex ml-2 mr-5 justify-center items-center w-17 p-2 bg-[#18A44C] rounded-md hover:bg-[#00E154] hover:cursor-pointer">
                    <ArrowRightOnRectangleIcon className="h-full w-auto " />
                      </Link>
               </div>
                ): ( 
                    <div className="flex flex-row rounded-3xl items-center gap-10">
                        <Link href="/auth/login">Login</Link>
                        <div className="bg-[#FF7F2A] p-2 rounded">
                            <Link href="/auth/signup">Sign Up</Link>
                        </div>
                    </div>
                )}
              </div>
                
            </div>
            <Divider />
        </div>
    );
}
