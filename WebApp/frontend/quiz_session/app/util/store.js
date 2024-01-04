"use client";

import { HubConnectionBuilder } from "@microsoft/signalr";
import { create } from "zustand";

export const useConnection = create((set) => ({
  connection: new HubConnectionBuilder()
    .withUrl("https://localhost:7081/gateway/hub/session")
    .build(),
}));

export const useConnectionId = create((set) => ({
  connectionId: "",
  setConnection: (id) => set({ connectionId: id }),
}));
