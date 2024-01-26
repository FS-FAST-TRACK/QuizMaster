"use client";
import React from "react";
import Link from "next/link";
import { useSession } from "next-auth/react";
import CryptoJS, { enc } from "crypto-js";

export default function Game() {
    const { data: session } = useSession();
    const user = session?.user;

    function encodeUTF8(input: string) {
        return encodeURIComponent(input);
    }

    // Get the data to be encrypted (e.g., from localStorage)
    const token = localStorage.getItem("token") || "";

    // Encode the data as UTF-8
    const utf8EncodedToken = encodeUTF8(token);

    // Encrypt the UTF-8 encoded token using CryptoJS AES
    //TODO: SECRET KEY SHOULD BE REPLACED
    const encryptedToken = CryptoJS.AES.encrypt(
        utf8EncodedToken,
        "secret_key"
    ).toString();

    const encodedEncryptedToken = encodeURIComponent(encryptedToken);
    return (
        <Link
            href={`http://localhost:3001?name=${user?.username}&token=${encodedEncryptedToken}`}
        >
            {user?.username}
        </Link>
    );
}
