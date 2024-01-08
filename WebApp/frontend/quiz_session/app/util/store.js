"use client";

import { HubConnectionBuilder } from "@microsoft/signalr";
import { create } from "zustand";

//connection
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

//connection ID
export const useConnectionId = create((set) => ({
  connectionId: "",
  setConnectionId: (id) => set({ connectionId: id }),
}));

//join room
export const useParticipants = create((set) => ({
  participants: [],
  setParticipants: (players) => {
    set({ participants: players });
  },
}));

//chat
export const useChat = create((set) => ({
  chat: undefined,
  setChat: (message) => {
    set({ chat: message });
  },
}));

//start
export const useStart = create((set) => ({
  isStart: false,
  setStart: (start) => {
    set({ isStart: start });
  },
}));

//question
export const useQuestion = create((set) => ({
  question: undefined,
  setQuestion: (q) => {
    set({ question: q });
  },
}));

//leaderboard
export const useLeaderboard = create((set) => ({
  leader: [],
  isStop: false,
  setLeaderboard: ({ scores, stop }) => {
    set({ leader: scores, isStop: stop });
  },
  setResetLeader: () => {
    set({ leader: [] });
  },
}));
