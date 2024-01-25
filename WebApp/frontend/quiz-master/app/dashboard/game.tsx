"use client";
import React from "react";
import Link from "next/link";
import { useSession } from "next-auth/react";
import CryptoJS from "crypto-js";

export default function Game() {
    const { data: session } = useSession();
    const user = session?.user;
    const encryptedToken: string = CryptoJS.AES.encrypt(
        localStorage.getItem("token") || "",
        "secret_key"
    ).toString();
    return (
        <Link
            href={`http://localhost:3001?name=${user?.username}&token=${encryptedToken}`}
        >
            {encryptedToken.valueOf()}
        </Link>
    );
}
