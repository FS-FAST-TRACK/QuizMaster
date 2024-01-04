import React from "react";
import logo1 from "@/public/logo/Logo1.svg";
import Image from "next/image";
import copy from "@/public/icons/copy.png";
import { Loader, Button } from "@mantine/core";
import { names } from "../util/data";
import RoomPin from "./roomPin";
import Participants from "./participants";
import Start from "./start";

export default function Room() {
  return (
    <div className=" w-full h-full p-5 flex flex-col">
      <RoomPin />
      <div className=" grow flex items-center flex-col flex-1   overflow-hidden">
        <Participants />
      </div>
      <Start />
    </div>
  );
}
