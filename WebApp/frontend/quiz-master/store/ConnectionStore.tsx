import { create } from "zustand";
import {
    HubConnection,
    HubConnectionBuilder,
    LogLevel,
} from "@microsoft/signalr";
import { QUIZMASTER_GATEWAY_SESSION_HUB } from "@/api/api-routes";

interface IConnectionStore {
    connection?: HubConnection;
    setConnection: () => void;
    init: () => void;
    startConnection: () => void;
    loginConnection: (token: string) => void;
}

export const useConnectionStore = create<IConnectionStore>((set, get) => ({
    connection: undefined,
    setConnection: () => {},
    init: () => {
        if (!get().connection) {
            console.log("Creating connection to gateway session hub.");
            set({
                connection: new HubConnectionBuilder()
                    .withUrl(QUIZMASTER_GATEWAY_SESSION_HUB)
                    .configureLogging(LogLevel.Information)
                    .build(),
            });
        }
    },
    startConnection: () => {
        if (get().connection !== undefined) {
            console.log("Staring connection...");
            get().connection?.start();
            console.log("Connection started");
        }
    },
    loginConnection: (token: string) => {
        if (get().connection !== undefined) {
            get().connection?.invoke("login", token);
        }
    },
}));
