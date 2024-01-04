"use client";

import React from "react";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { useConnection, useConnectionId } from "@/app/util/store";

export default function Login() {
  const { connection } = useConnection();
  const { setConnection } = useConnectionId();
  const { push } = useRouter();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async (e) => {
    e.preventDefault();

    fetch("https://localhost:7081/gateway/api/auth/login", {
      method: "POST",
      credentials: "include",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password }),
    }).then(async (r) => {
      if (r.status === 200) {
        try {
          const data = await r.json();
          await connection.invoke("Login", data.token);
          localStorage.setItem("username", username.toLowerCase());

          push("/auth/code");
        } catch (error) {
          console.error("SignalR error:", error);
        }
      }
    });
  };
  return (
    <form onSubmit={handleLogin}>
      <label>Email</label>
      <input
        placeholder="example@ex.com"
        className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
        onChange={(e) => setUsername(e.target.value)}
      />
      <label>Username</label>
      <input
        placeholder="Code"
        className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
        onChange={(e) => setPassword(e.target.value)}
      />

      <button
        className="w-full bg-button py-2 text-white text-lg font-bold rounded-lg"
        type="submit"
      >
        Sign Up
      </button>
    </form>
  );
}
