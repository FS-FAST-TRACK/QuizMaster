"use client";

import React from "react";
import { Loader } from "@mantine/core";
import { useEffect, useState } from "react";
import { useParticipants } from "@/app/util/store";

export default function Participants() {
  const [names, setNames] = useState([]);
  const { participants } = useParticipants();

  useEffect(() => {
    setNames(participants);
  }, [participants]);

  return (
    <>
      <div className=" flex  flex-col items-center space-y-2">
        <Loader color="rgba(255, 255, 255, 1)" size="md" />
        <div className="text-white font-bold">Waiting for other players...</div>
      </div>
      <div className="flex flex-wrap justify-center w-full  overflow-auto ">
        {names?.map((name, index) => (
          <div
            key={index}
            className="bg-white py-2 px-5 rounded-lg text-green_text font-bold m-2"
          >
            {name.qParticipantDesc}
          </div>
        ))}
      </div>
    </>
  );
}
