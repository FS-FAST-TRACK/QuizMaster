"use client";

import { SessionProvider } from "next-auth/react";
import { PropsWithChildren } from "react";

import { Session } from "next-auth";

type AuthProviderProps = {
    children: React.ReactNode;
    session?: Session | null;
};

export const AuthProvider = ({
    children,
    session,
}: PropsWithChildren<AuthProviderProps>) => {
    return <SessionProvider session={session}>{children}</SessionProvider>;
};
