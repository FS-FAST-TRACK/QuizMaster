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

    const [isOpen, setIsOpen] = useState(false);

    const toggleNavbar = () => {
      setIsOpen(!isOpen);
    };

   
    return (

        <div className="flex flex-row gap-4">
        <div className="w-full flex mx-auto px-4 sm:px-6 lg:px-8 gap-0 h-10 text-white transition-all duration-500">
          <div className="flex justify-center flex-row rounded-3xl items-center gap-10  h-16">
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
            <div className="hidden sm:block">
              <div className="flex space-x-4">
              <Link href="/home" passHref>
              <span className="text-white hover:border-b-2 p-2 border-[#FF7F2A] font-bold cursor-pointer">Home</span>
            </Link>
            <Link href="/system-info" passHref>
              <span className="text-white hover:border-b-2 p-2 border-[#FF7F2A] font-bold cursor-pointer">About</span>
            </Link>
            <Link href="/contact-us" passHref>
              <span className="text-white hover:border-b-2 p-2 border-[#FF7F2A] font-bold cursor-pointer">Contact Us</span>
            </Link>
              </div>
            </div>
            <div style={{flexDirection:'row',flex:1,justifyContent:'space-between'}}>
            <div className="flex-grow flex-row justify-between space-x-0"></div>
            <div className="hidden sm:block">
                {/* Conditionally render based on user info */}
                <div style={{ display: loading ? "flex" : "none" }}>
            {userInfo ? (
              <div className="flex justify-between items-center">
                <Link className="flex flex-row justify-center items-center h-5 p-5 mr-2 w-40 p-5 bg-[#FF7F2A] hover:bg-[#FF7F2A80] gap-2 rounded-md hover:cursor-pointer" style={{ display: !userInfo?.info.roles.includes("Administrator") ? "none" : "flex" }} href="/dashboard"><b>Create Quiz</b></Link>
                <Popover position="bottom" withArrow shadow="md">
                  <Popover.Target>
                    <Button style={{ backgroundColor: "transparent" }} className="flex flex-row justify-center items-center h-5 p-5 mr-2 w-40 p-5 gap-2 rounded-md hover:cursor-pointer">
                      <div style={{ flexDirection: 'row' }}>
                        <text className="ml-2" style={{ fontSize: 18, fontWeight: 'bold' }}>{firstName ? firstName : userInfo?.info?.userData?.userName}</text>
                      </div>
                      <Image
                        src={chevronDown}
                        alt="chevron"
                        height={20}
                        style={{ color: 'white', height: '100%', marginTop: 2, marginLeft: 8 }}
                      />
                    </Button>
                  </Popover.Target>
                  <Popover.Dropdown style={{ paddingTop: 7, paddingBottom: 5,zIndex:999 }}>
                    <Link href="/profile" className="hover:bg-[#5a5a5a30] flex rounded-md hover:cursor-pointer p-3 text-center">Profile Settings</Link>
                    <Link href="/auth/signout" className="hover:bg-[#5a5a5a30] flex rounded-md p-3 hover:cursor-pointer text-center">Sign out</Link>
                  </Popover.Dropdown>
                </Popover>
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
              </div>
              </div>
            <div className="flex justify-between items-center">
              
              <div className="-mr-2 flex sm:hidden">
                <button
                  onClick={() => setIsOpen(!isOpen)}
                  className="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white"
                >
                  <span className="sr-only">Open main menu</span>
                  {!isOpen ? (
                        <svg className="block h-6 w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="white">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16m-7 6h7" />
                        </svg>
                        ) : (
                        <svg className="block h-6 w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="white">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" />
                        </svg>
                        )}
                </button>
              </div>
            </div>
          </div>
        </div>
        {/* Mobile menu */}
        {isOpen && (
          <div className="sm:hidden">
            <div className="px-2 pt-2 pb-3 space-y-1">
              <Link href="/home" passHref>
                <span className="text-white block px-3 py-2 rounded-md text-base font-medium">
                  Home
                </span>
              </Link>
              <Link href="/system-info" passHref>
                <span className="text-white block px-3 py-2 rounded-md text-base font-medium">
                  About
                </span>
              </Link>
              <Link href="/contact-us" passHref>
                <span className="text-white block px-3 py-2 rounded-md text-base font-medium">
                  Contact Us
                </span>
              </Link>
              {/* Add the rest of the mobile menu items */}
            </div>
          </div>
        )}
      </div>
        
    );
}
