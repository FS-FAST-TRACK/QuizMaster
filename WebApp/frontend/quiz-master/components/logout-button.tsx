"use client";

import { Button } from "@mantine/core";
import { signOut, useSession } from "next-auth/react";
import { cookies } from "next/headers";

const LogoutButton = () => {
    const { data: session } = useSession();
    console.log(session);
    const logout = async () => {
        await fetch(`${process.env.QUIZMASTER_GATEWAY}/auth/logout`, {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            },
        });
        await signOut();
    };
    return <Button onClick={logout}>Logout</Button>;
};

export default LogoutButton;
