import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";

export default function page() {
  return (
    <div className="w-96 h-96 bg-white flex flex-col items-center p-6 rounded-xl absolute top-10">
      <Image src={logo1} className="w-40" />
      <div className="w-full flex items-center flex-col space-y-2 grow  justify-center">
        <div className="text-xl font-bold">Join Room</div>
        <div className="text-center text-secondary_text">
          Enter the 8-digit code for the quiz room
        </div>
      </div>
      <div className=" space-y-2 ">
        <input
          placeholder="Code"
          className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
        />
        <button className="w-full bg-button py-2 text-white text-lg font-bold rounded-lg">
          Join Room
        </button>
      </div>
    </div>
  );
}
