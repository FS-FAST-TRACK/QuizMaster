import React from 'react';
import personSvg from '/public/person.svg';
import Image from "next/image";
import HeroSection from './sectionone';
export default function home() {
    
  return (
    <div className='flex flex-col '>
      <section className="relative overflow-hidden h-screen">
        <div className="absolute inset-0 z-0 "></div>
        <div className="relative z-10 flex flex-col items-center justify-center text-white">
        <HeroSection />
        </div>
      </section>

      <section className="relative overflow-hidden h-screen">
        <div className="absolute inset-0 z-0 bg-black bg-opacity-50"></div>
        <div className="relative z-10 flex flex-col items-center justify-center text-white">
          <h1 className="text-4xl font-bold">Hero Section 2</h1>
          <p className="text-lg">Subtitle for Hero Section 2</p>
        </div>
      </section>
    </div>
  );
}

 