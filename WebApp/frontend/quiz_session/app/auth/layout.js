import React from "react";
import Image from "next/image";
import logo2 from "@/public/logo/Logo2.svg";

export default function layout({ children }) {
  return (
    <div className="w-full h-full justify-center pt-24 flex relative overflow-hidden">
      <Image
        src={logo2}
        className="absolute top-20 left-64 transform -translate-x-1/2 w-screen h-screen  "
      />
      {children}
    </div>
  );
}
