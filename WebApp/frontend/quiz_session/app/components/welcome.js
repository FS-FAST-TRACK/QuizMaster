"use client";

import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import { useEffect } from "react";
import { ConnectToHub } from "../util/Connections";
import { useConnection } from "../util/store";

export default function Welcome() {
  const { connection } = useConnection();
  if (!connection.state !== "Connected") {
    connection
      .start()
      .then(() => {
        console.log("Connection started");
        connection.on("participants", participants);
      })
      .catch((err) => {
        console.error("Error starting connection:", err);
      });
  }

  return (
    <>
      {/* <div className="bg-white w-1/4 h-1/4 rounded-lg">
        <Image
          src={logo1}
          alt="Quis Master Logo"
          className="w-full h-full p-5"
        />
      </div> */}
    </>
  );
}
