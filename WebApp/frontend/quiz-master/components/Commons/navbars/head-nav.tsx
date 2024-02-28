"use client";
import Link from "next/link";
import React from 'react';
import Image from "next/image";
import logo from "/public/quiz-master-logo-white.png";
import { useEffect, useState } from "react";
import { fetchLoginUser } from "@/lib/quizData";
import { UserInfo } from "@/lib/definitions";
import {Popover,Button } from '@mantine/core';
import chevronDown from "/public/chevronDown.svg";
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
    const firstName = userInfo?.info?.userData?.firstName;
    const lastName =  userInfo?.info?.userData?.lastName;
 
    return (
        <div className="flex flex-col gap-4 ">
            <div className="flex flex-row w-full gap-10 h-10 text-white transition-all duration-500">
                <div className="flex flex-row rounded-3xl items-center gap-10">
                    <Link href={"/home"} className="hidden lg:block">
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
                    <Link href="/home"><b>Home</b></Link>
                  
                    <Link href="/system-info"><b>About</b></Link>
                    <Link  href="/contact-us"><b>Contact Us</b></Link>
                </div>
                <div  className="flex grow flex-row justify-between space-x-0"></div>
              
              <div style={{display:loading?"flex":"none"}}>
              {userInfo ? (
             
                 <div className="flex justify-between items-center">
                 <Link className=" flex flex-row justify-center items-center  h-5 p-5 mr-2 w-40 p-5 bg-[#FF7F2A] gap-2 rounded-md  hover:cursor-pointer" style={{display:!userInfo?.info.roles.includes("Administrator")?"none":"flex"}} href="/dashboard"><b>Create Quiz</b></Link>  
                
                 <Popover position="bottom" withArrow shadow="md" >
                    <Popover.Target >
      
                        <Button  style={{backgroundColor:"transparent"}}  className=" flex flex-row justify-center items-center  h-5 p-5 mr-2 w-40 p-5  gap-2 rounded-md hover:bg-white hover:cursor-pointer" >
                            <div style={{flexDirection:'row'}}>    
                         <text className="ml-2 " >{firstName ? firstName : userInfo?.info?.userData?.userName } </text>
                         </div>
                         <Image
                                src={chevronDown}
                                alt="chevron"
                                height={20}
                                style={{color:'white',height:'100%',marginTop:2,marginLeft:5}}
                            
                            />
                         </Button>
                    </Popover.Target>
                    <Popover.Dropdown style={{paddingTop:7,paddingBottom:5,}}  >
                    <Link    href="/profile" className="hover:bg-amber-200 flex rounded-md  hover:cursor-pointer p-3 text-center"
                            >
                               Profile Settings
                                </Link>
                                <Link href="/auth/signout" className="hover:bg-amber-200 flex rounded-md p-3 hover:cursor-pointer text-center">
                                       Sign out 
                                    </Link>
                    </Popover.Dropdown>
                </Popover>
           
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
         
        </div>
    );
}
