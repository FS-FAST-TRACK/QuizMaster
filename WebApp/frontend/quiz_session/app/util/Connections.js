"use client";

import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useConnection } from "./store";

let connection;

export function ConnectToHub() {
  try {
    const { connection } = useConnection();
    connection.start();
  } catch (ex) {
    console.error(ex);
  }
}

export async function Login(token) {
  const { connection } = useConnection();
  await connection.invoke("Login", token);
}
