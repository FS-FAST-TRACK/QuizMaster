"use client";
import React from "react";
import Link from "next/link";
import { useSession } from "next-auth/react";
<<<<<<< HEAD
import CryptoJS, { enc } from "crypto-js";
=======
import CryptoJS from "crypto-js";
>>>>>>> quiz_session_UI

export default function Game() {
    const { data: session } = useSession();
    const user = session?.user;
<<<<<<< HEAD

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
=======
    const encryptedToken: string = CryptoJS.AES.encrypt(
        localStorage.getItem("token") || "",
        "secret_key"
    ).toString();
    return (
        <Link
            href={`http://localhost:3001?name=${user?.username}&token=${encryptedToken}`}
>>>>>>> quiz_session_UI
        >
            {encryptedToken.valueOf()}
        </Link>
    );
}
