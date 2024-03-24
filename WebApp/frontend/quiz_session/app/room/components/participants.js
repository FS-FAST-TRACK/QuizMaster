"use client";

import React from "react";
import { Loader } from "@mantine/core";
import { useEffect, useState } from "react";
import { useAnsweredParticipants, useParticipants } from "@/app/util/store";

export default function Participants({
  includeLoaderModal = true,
  className = "",
  excludeAdmins = false,
}) {
  const [names, setNames] = useState([]);
  const { participants } = useParticipants();
  const { answeredParticipants } = useAnsweredParticipants();

  useEffect(() => {
    if (excludeAdmins) setNames(participants.filter((p) => !p.isAdmin));
    else setNames(participants);
  }, [participants, excludeAdmins]);

  return (
    <>
      {includeLoaderModal && (
        <div
          className={`flex  flex-col items-center space-y-2 mt-8 mb-8 ${className}`}
        >
          <Loader color="rgba(255, 255, 255, 1)" size="md" />
          <div className="text-white font-regular">
            Waiting for other players...
          </div>
        </div>
      )}

      {!includeLoaderModal && (
        <div className="text-white font-regular py-8 mt-4 text-center">
          Participants
        </div>
      )}
      <div
        className={`flex flex-wrap justify-center w-full  overflow-auto ${className}`}
      >
        {names?.map((name, index) => (
          <div
            key={index}
            className={`py-2 px-5 rounded-md font-bold m-2 shadow-sm ${
              answeredParticipants.includes(name.qParticipantDesc)
                ? "bg-yellow-500 text-white"
                : "text-white bg-green-700/50"
            }`}
          >
            {name.qParticipantDesc.toUpperCase()}
          </div>
        ))}
      </div>
    </>
  );
}
