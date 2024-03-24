"use client";

import React from "react";
import { useConnection } from "@/app/util/store";
import { useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import { submitAnswer } from "@/app/util/api";
import { submitPin } from "../../util/handlers";

export default function Code() {
  const { connection } = useConnection();
  const [code, setCode] = useState();
  const { push } = useRouter();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  const handleSubmit = (e) => {
    e.preventDefault();
    submitPin(connection, code, params, push);
  };

  return (
    <div className=" space-y-2 ">
      <form onSubmit={handleSubmit}>
        <input
          placeholder="Code"
          className="w-full p-2 border-gray-300 border-[1px] rounded-md"
          onChange={(e) => {
            setCode(e.target.value);
          }}
          autoFocus={true}
        />
        <button
          className="w-full bg-button py-2 text-white text-lg font-bold rounded-md mt-3"
          type="submit"
        >
          Join Room
        </button>
      </form>
    </div>
  );
}
