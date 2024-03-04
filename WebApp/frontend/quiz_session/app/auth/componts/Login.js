"use client";

import React from "react";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { useConnection, useConnectionId } from "@/app/util/store";
import { notifications } from "@mantine/notifications";
import { partialLogin } from "@/app/util/api";

export default function Login() {
  const { connection } = useConnection();
  const { setConnection } = useConnectionId();
  const { push } = useRouter();
  const [email, setEmail] = useState("");
  const [userName, setUserName] = useState("");

  const handleLogin = async (e) => {
    e.preventDefault();
    partialLogin({ email, userName, connection, push, notifications });
  };
  return (
    <form onSubmit={handleLogin} className="space-y-4">
      <div>
        <label className=" text-secondary_text text-sm">Email</label>
        <input
          placeholder="example@example.com"
          className="w-full p-2 border-gray-300 border-[1px] rounded-md"
          onChange={(e) => setEmail(e.target.value)}
          autoFocus={true}
        />
      </div>

      <div>
        <label className=" text-secondary_text text-sm">Username</label>
        <input
          placeholder="username"
          className="w-full p-2 border-gray-300 border-[1px] rounded-md"
          onChange={(e) => setUserName(e.target.value)}
        />
      </div>
      <div className="h-4"></div>
      <button
        className="w-full bg-button py-2 text-white text-base font-semibold rounded-md"
        type="submit"
      >
        Sign Up
      </button>
    </form>
  );
}
