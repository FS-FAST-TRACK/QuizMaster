"use client";
import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import copy from "@/public/icons/copy.png";
import { useSearchParams } from "next/navigation";

export default function RoomPin() {
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);
  return (
    <div className=" flex-row flex h-14 space-x-2">
      <div className="bg-white p-2 rounded-lg h-full justify-center items-center flex">
        <Image src={logo1} height={30} alt="logo" />
      </div>
      <div className="flex h-full w-fit flex-col justify-between">
        <h6 className="text-white text-sm">Room Code</h6>
        <div className="flex flex-row  w-full space-x-2">
          <h6 className="text-white text-2xl font-bold">
            {params.get("roomPin")}
          </h6>
        </div>
      </div>
    </div>
  );
}
