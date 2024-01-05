"use client";

import { HubConnectionBuilder } from "@microsoft/signalr";
import { create } from "zustand";

export const useConnection = create((set, get) => ({
  connection: undefined,

  setConnection: () => {
    if (get().connection !== undefined) console.log("initializing");
    set({
      connection: new HubConnectionBuilder()
        .withUrl("https://localhost:7081/gateway/hub/session")
        .build(),
    });
  },
}));

// const useConnection = create((set) => {
//   let connectionInstance;

//   const createConnection = () => {
//     return new HubConnectionBuilder()
//       .withUrl("https://localhost:7081/gateway/hub/session")
//       .build();
//   };

//   set({
//     connection: state.connection ?? createConnection(),
//   });
// });

export const useConnectionId = create((set) => ({
  connectionId: "",
  setConnection: (id) => set({ connectionId: id }),
}));
