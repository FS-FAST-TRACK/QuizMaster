"use client"
import HeadNav from "@/components/Commons/navbars/head-nav";
import waveSvg from '/public/wave.svg';
import Image from "next/image";
import personSvg from '/public/person.svg'; // Import the SVG file
import HeroSection from './sectionone';
import React,{useEffect,useState} from "react";
export default function Layout({ children }: { children: React.ReactNode }) {
  const [isScreenLarge, setIsScreenLarge] = useState(false);

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
    return (
      <div className="flex flex-col w-full h-full text-white bg-gradient-to-r from-[#17A14B] to-[#1AC159] gap-16 " style={{overflow:'scroll',overflowY:'auto'}}>
      <div className="pt-8 pl-5 pr-8 md:px-20 lg:px-15">
        <HeadNav />
      </div>
      <div className="flex flex-col">
        {/* First Section */}
        <section className="relative ">
          <div className="h-screen">
            <HeroSection />
          </div>
          {/* Use the SVG as a background */}
          <div className="absolute bottom-0 flex flex-col w-full hidden lg:block">
            <svg viewBox="0 0 1440 274" fill="none" xmlns="http://www.w3.org/2000/svg" className="h-auto">
              <path
                fill="#ffffff"
                fillRule="evenodd"
                clipRule="evenodd"
                d="M0,64L48,64C96,64,192,64,288,74.7C384,85,480,107,576,138.7C672,171,768,213,864,208C960,203,1056,149,1152,138.7C1248,128,1344,160,1392,176L1440,192L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z"
              ></path>
            </svg>
            <div style={{ height: !isScreenLarge ? 60 : 100, backgroundColor: 'white' }} className="hidden xl:block"></div>
          </div>
          {/* Div with white background and 400px height */}
        </section>
      
        <div className="pl-8 md:pl-20 -mt-15 bg-white hidden sm:block ">
          <p className="text-sm text-black text-center">
            Copyright 2023 Ⓒ QuizMaster
          </p>
        </div>
      </div>
    </div>
    
    );
}
