"use client";
import React from 'react';
import personSvg from '/public/person.svg'; // Import the SVG file
import Image from 'next/image';
import Link from "next/link";
import { useSession } from "next-auth/react";
import CryptoJS from "crypto-js";
import { QUIZMASTER_SESSION_WEBSITE } from "@/api/api-routes";

const HeroSection = () => {
    const { data: session } = useSession();
    const user = session?.user;

    function encodeUTF8(input: string) {
        return encodeURIComponent(input);
    }

    // Get the data to be encrypted (e.g., from localStorage)
    const token = localStorage.getItem("token") || "";

    // Encode the data as UTF-8
    const utf8EncodedToken = encodeUTF8(token);

    // Encrypt the UTF-8 encoded token using CryptoJS AES
    //TODO: SECRET KEY SHOULD BE REPLACED
    const encryptedToken = CryptoJS.AES.encrypt(
        utf8EncodedToken,
        "secret_key"
    ).toString();

    const encodedEncryptedToken = encodeURIComponent(encryptedToken);
    return (
        <div className="flex-row flex -mt-40">
        <div className="flex flex-row items-center"> {/* Adjusted structure */}
            <div>
                <h1 className="text-7xl font-bold text-white">
                    Unlocking Your Inner QuizMaster.
                </h1>
                <p className='mt-5 text-lg text-white'>Ignites friendly competition and knowledge exploration, fostering a community of champions and lifelong learners through an engaging platform for intellectual development and inclusive learning.</p>
                <Link
                    href={`${QUIZMASTER_SESSION_WEBSITE}?name=${user?.username}&token=${encodedEncryptedToken}`}
                    type="button"
                    style={{}}
                    className="mt-5 text-white rounded bg-[#FFAD33]  hover:bg-[#FFAD3390] text-bold px-6 pb-2 pt-2.5 text-md font-medium uppercase">
                    JOIN A ROOM
                </Link>
            </div>
            <Image
                src={personSvg}
                alt="QuizMaster Icon"
                width={"3000"}
                height={"3000"}
            />
        </div>
    </div>
       
                   
  

    );
}

export default HeroSection;