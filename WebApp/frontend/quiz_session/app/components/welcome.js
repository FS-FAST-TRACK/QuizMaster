"use client";

import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import { useEffect } from "react";
import { ConnectToHub } from "../util/Connections";
import { useConnection } from "../util/store";
import { notifications } from "@mantine/notifications";

export default function Welcome() {
  const { connection, setConnection } = useConnection();
  useEffect(() => {
    setConnection();
  }, []);
  useEffect(() => {
    console.log(connection);
    if (connection !== undefined && connection.state !== "Connected") {
      console.log("connected");
      connection
        .start()
        .then(() => {
          console.log("Connection started");
          //connection.on("participants", participants);
          connection.on("notif", (message) => {
            console.log("notification from welcome");
            notifications.show({
              title: message + "notification from welcome",
            });
          });
        })
        .catch((err) => {
          console.error("Error starting connection:", err);
        });
    }
  }, [connection]);

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
