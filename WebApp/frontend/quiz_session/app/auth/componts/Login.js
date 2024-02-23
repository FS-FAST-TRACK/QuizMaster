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
        <label>Email</label>
        <input
          placeholder="example@example.com"
          className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
          onChange={(e) => setEmail(e.target.value)}
          autoFocus={true}
        />
      </div>

      <div>
        <label>Username</label>
        <input
          placeholder="username"
          className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
          onChange={(e) => setUserName(e.target.value)}
        />
      </div>

      <button
        className="w-full bg-button py-2 text-white text-lg font-bold rounded-lg"
        type="submit"
      >
        Sign Up
      </button>
    </form>
  );
}
