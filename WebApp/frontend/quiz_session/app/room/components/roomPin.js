"use client";
import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import copy from "@/public/icons/copy.png";
import { useSearchParams } from "next/navigation";
import { BASE_URL } from "@/app/util/api";
import { headers } from "@/next.config";

export default function RoomPin() {
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  // Save Room Information
  const SaveRoomInfo = () => {
    const _Data = localStorage.getItem("_rI");
    if(!_Data){
      const token = localStorage.getItem("token");
      fetch(BASE_URL+`/gateway/api/room/getRoomByPin/${params.get("roomPin")}`, {
        headers:{"Content-Type":"application/json", "Authorization": `Bearer ${token}`},
        credentials: "include"
      }).then(r => r.json())
      .then(d => {
        const room = JSON.stringify(d.data);
        localStorage.setItem("_rI", room);
  
      }).catch(e => {console.error("Failed to save room information: ", e)})
    }
  }
  SaveRoomInfo();
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
