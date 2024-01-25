import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import StartConnection from "./components/welcome";

export default function page() {
  return (
    <div className="w-full h-full justify-center items-center flex">
      <div className="bg-white w-1/4 h-1/4 rounded-lg">
        <StartConnection />
        <Image
          src={logo1}
          alt="Quis Master Logo"
          className="w-full h-full p-5"
        />
      </div>
    </div>
  );
}
