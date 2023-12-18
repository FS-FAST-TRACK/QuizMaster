"use client";

import React from "react";
import { useConnection } from "@/app/util/store";
import { useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";

export default function Code() {
  const { connection } = useConnection();
  const [code, setCode] = useState();
  const { push } = useRouter();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  const handleSubmit = (e) => {
    e.preventDefault();
    try {
      connection.invoke("JoinRoom", Number.parseInt(code));
      params.set("roomPin", Number.parseInt(code));
      push(`/room?${params.toString()}`);
    } catch (ex) {
      console.log(ex);
    }
  };

  return (
    <div className=" space-y-2 ">
      <form onSubmit={handleSubmit}>
        <input
          placeholder="Code"
          className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
          onChange={(e) => {
            setCode(e.target.value);
          }}
        />
        <button
          className="w-full bg-button py-2 text-white text-lg font-bold rounded-lg"
          type="submit"
        >
          Join Room
        </button>
      </form>
    </div>
  );
}
