"use client";

import { SessionProvider } from "next-auth/react";
import { PropsWithChildren } from "react";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import { Session } from "next-auth";

const queryClient = new QueryClient();

type AuthProviderProps = {
    children: React.ReactNode;
    session?: Session | null;
};

export const AuthProvider = ({
    children,
    session,
}: PropsWithChildren<AuthProviderProps>) => {
    return (
        <SessionProvider session={session}>
            <QueryClientProvider client={queryClient}>
                {children}
            </QueryClientProvider>
        </SessionProvider>
    );
};
