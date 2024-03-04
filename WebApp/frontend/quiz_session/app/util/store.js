"use client";

import { HubConnectionBuilder } from "@microsoft/signalr";
import { create } from "zustand";

export const BASE_URL = process.env.NEXT_PUBLIC_QUIZMASTER_GATEWAY;

//connection
export const useConnection = create((set, get) => ({
  connection: undefined,

  setConnection: () => {
    if (get().connection !== undefined) console.info("initializing");
    set({
      connection: new HubConnectionBuilder()
        .withUrl(`${BASE_URL}/gateway/hub/session`)
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

export const useMetaData = create((set) => ({
  metadata: undefined,
  setMetadata: (data) => {
    set({ metadata: data });
  },
}));

// answer
export const useAnswer = create((set) => ({
  answer: undefined,
  setAnswer: (data) => {
    set({ answer: data });
  }
}))
