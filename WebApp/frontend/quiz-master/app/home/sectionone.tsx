"use client";
import React,{useState,useEffect} from 'react';
import personSvg from '/public/person.svg'; // Import the SVG file
import homewave from '/public/homewave.svg'; // Import the SVG file
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
  
    const [isScreenLarge, setIsScreenLarge] = useState(false);
    const [isScreenSmall, setIsScreenSmall] = useState(false); 

    useEffect(() => {
      const handleResize = () => {
        setIsScreenSmall(window.innerWidth >= (3 * window.screen.width));
      };
  
      // Initial check on mount
      handleResize();
  
      // Listen for window resize events
      window.addEventListener('resize', handleResize);
  
      // Cleanup
      return () => {
        window.removeEventListener('resize', handleResize);
      };
    }, []);

    
    useEffect(() => {
      const handleResize = () => {
        setIsScreenLarge(window.innerWidth >= (0.8 * window.screen.width));
      };
  
      // Initial check on mount
      handleResize();
  
      // Listen for window resize events
      window.addEventListener('resize', handleResize);
  
      // Cleanup
      return () => {
        window.removeEventListener('resize', handleResize);
      };
    }, []);

    const [windowWidth, setWindowWidth] = useState(0);
    const [imageWidth, setImageWidth] = useState(0);
    useEffect(() => {
      const handleResize = () => {
        setWindowWidth(window.innerWidth);
      };
  
      // Set initial window width
      setWindowWidth(window.innerWidth);
  
      window.addEventListener('resize', handleResize);
  
      return () => {
        window.removeEventListener('resize', handleResize);
      };
    }, []);
    useEffect(() => {
      // Calculate image width based on window width
      const calculateImageWidth = () => {
        // Adjust the value as needed based on your design
        const maxWidth = 1500;
        const minWidth = 1500;
        const scaleFactor = 1;
  
        const calculatedWidth = windowWidth * scaleFactor;
        setImageWidth(Math.min(maxWidth, Math.max(minWidth, calculatedWidth)));
      };
  
      calculateImageWidth();
  
      // Recalculate image width on window width change
      window.addEventListener('resize', calculateImageWidth);
  
      return () => {
        window.removeEventListener('resize', calculateImageWidth);
      };
    }, [windowWidth]);

    return (
        <div className="flex items-center justify-center min-h-screen -mt-5">
        <div className="flex flex-col md:flex-row items-center md:items-start gap-8 pl-4 lg:pl-20">
          {/* 1st column row text */}
          <div className="flex flex-col md:flex-row items-center md:items-start gap-8 pl-4 lg:pl-20">
  {/* 1st column row text */}
  <div className="bottom-0 flex flex-col flex-grow" >
    <h1 className={`text-4xl  md:text-3xl xl:text-7xl lg:-ml-10 lg:mr-10 lg:-mt-10 lg:text-6xl md:ml-10 md:-mt-10  font-bold text-white`} >
      Unlocking Your Inner QuizMaster.
    </h1>
    <p className="text-white mt-2  lg:mr-10  lg:-ml-8 lg:text-md md:ml-10 xl:-ml-8 xl:text-lg xl:mr-5 md:mr-10 ">
      Ignites friendly competition and knowledge exploration, fostering
      a community of champions and lifelong learners through an engaging
      platform for intellectual development and inclusive learning.
    </p>
    <Link
      style={{zIndex:999,maxWidth:160}}
      href={`${QUIZMASTER_SESSION_WEBSITE}?name=${user?.username}&token=${encodedEncryptedToken}`}
      className="mt-5 text-white lg:-ml-8 xl:-ml-8 md:ml-10  rounded bg-[#FFAD33] hover:bg-[#FFAD3390] text-bold px-6 pb-2 pt-2.5 text-md font-medium uppercase "
    >
      JOIN A ROOM
    </Link>
  </div>
</div>

          {/* 2nd column Image */}
          <div className={!isScreenLarge || isScreenSmall ? "hidden" : "block lg:block"}>
          <Image 
            src={personSvg} 
            style={{ marginTop: -160, backgroundColor: 'transparent' }} 
             width={imageWidth}
             height={imageWidth}
            alt="Person"  
          />
        </div>
        </div>
      </div>
    );
}

export default HeroSection;