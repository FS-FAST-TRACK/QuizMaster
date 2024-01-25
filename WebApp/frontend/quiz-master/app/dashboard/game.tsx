"use client";
import React from "react";
import Link from "next/link";
import { useSession } from "next-auth/react";

export default function Game() {
    const { data: session } = useSession();
    const user = session?.user;
    return (
        <Link
            href={`http://localhost:3001?name=${user?.username}&token=${localStorage.getItem(
                "token"
            )}`}
        >
            {user?.username}
        </Link>
    );
}
