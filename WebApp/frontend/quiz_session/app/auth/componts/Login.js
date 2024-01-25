"use client";

import React from "react";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { useConnection, useConnectionId } from "@/app/util/store";
import { notifications } from "@mantine/notifications";

export default function Login() {
  const { connection } = useConnection();
  const { setConnection } = useConnectionId();
  const { push } = useRouter();
  const [email, setEmail] = useState("");
  const [userName, setUserName] = useState("");

  const handleLogin = async (e) => {
    e.preventDefault();

    fetch("https://localhost:7081/gateway/api/account/create_partial", {
      method: "POST",
      credentials: "include",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, userName }),
    }).then((r) => {
      if (r.status === 200) {
        try {
          fetch("https://localhost:7081/gateway/api/auth/partialLogin", {
            method: "POST",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, userName }),
          }).then(async (r) => {
            if (r.status === 200) {
              const data = await r.json();
              await connection.invoke("Login", data.token);
              localStorage.setItem("username", userName.toLowerCase());

              push("/auth/code");
            }
          });
        } catch (error) {
          console.error("SignalR error:", error);
        }
      } else {
        notifications.show({
          title: "Email or username already used ",
        });
      }
    });
  };
  return (
    <form onSubmit={handleLogin}>
      <label>Email</label>
      <input
        placeholder="example@example.com"
        className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
        onChange={(e) => setEmail(e.target.value)}
        autoFocus={true}
      />
      <label>Username</label>
      <input
        placeholder="username"
        className="w-full p-2 border-gray-300 border-[1px] rounded-lg"
        onChange={(e) => setUserName(e.target.value)}
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
