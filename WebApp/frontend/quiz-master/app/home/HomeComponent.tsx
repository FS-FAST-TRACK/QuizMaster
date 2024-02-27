import React from 'react';
import phone from "/public/quizmasterphone.png";
import Image from "next/image";
function HomeComponent() {
    
  return (
    <div className='flex'>
      <div className="flex flex-col items-center justify-center" style={{ minHeight: '50vh', position: 'relative' }}>
        <div style={{ display: 'flex', width: '100%' }}>
          <div style={{ flex: 1 }}>
            <h1 className="text-7xl font-bold text-white">
              Unlocking Your Inner QuizMaster.
            </h1>
            <h4 className='mt-5 text-lg text-white'>Ignites friendly competition and knowledge exploration, fostering a community of champions and lifelong learners through an engaging platform for intellectual development and inclusive learning.</h4>
          <div>
          <button
            type="button"
            className="mt-5 text-white rounded bg-[#FFAD33] hover:bg-[#F8C95B] px-6 pb-2 pt-2.5 text-md font-medium uppercase">   JOIN A ROOM
          
            </button>
       
          </div>
          </div>
          <div style={{ flex: 1, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <Image
              src={phone}
              alt="phone"
              width={350}
              height={350}
            />
          </div>
        </div>
      
      </div>

      
    </div>
  );
}

export default HomeComponent;