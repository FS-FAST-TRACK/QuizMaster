import useUserTokenData from "@/app/util/useUserTokenData";
import { Badge } from "@mantine/core";
import { IconCrown } from "@tabler/icons-react";
import React, { useEffect } from "react";
import { useState } from "react";

export default function ParticipantName() {
  const { isAdmin } = useUserTokenData();

  return (
    <div
      className={`px-4 py-2 bg-green-700/50 justify-center items-center rounded-md absolute mt-3 left-5 gap-4 min-w-[150px]`}
    >
      {isAdmin && (
        <div className="absolute -right-2 -top-2 bg-yellow-400 rounded-full p-1 shadow-md">
          <IconCrown size={18} color="white" />
        </div>
      )}
      <p className="text-xs font-light text-white opacity-75">
        {isAdmin ? "Host" : "Participant"}
      </p>
      <p className="text-base text-white font-medium">
        {localStorage.getItem("username") || "<No username>"}
      </p>
    </div>
  );
}
