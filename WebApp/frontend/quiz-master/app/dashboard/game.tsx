"use client";
import { useSession } from "next-auth/react";
import CryptoJS from "crypto-js";
import { QUIZMASTER_SESSION_WEBSITE } from "@/api/api-routes";
import jwtDecode from "jwt-decode";

export default function Game() {
    const { data: session } = useSession();
    let user = session?.user;
    let username;

    function encodeUTF8(input: string) {
        return encodeURIComponent(input);
    }

    // Get the data to be encrypted (e.g., from localStorage)
    const token = localStorage.getItem("token") || "";

    // Get username from token
    if (token) {
        const data: any = jwtDecode(token);
        const userData = JSON.parse(data.token);
        console.log(userData);
        username = userData["UserData"]["UserName"];
    }

    // Encode the data as UTF-8
    const utf8EncodedToken = encodeUTF8(token);

    // Encrypt the UTF-8 encoded token using CryptoJS AES
    //TODO: SECRET KEY SHOULD BE REPLACED
    const encryptedToken = CryptoJS.AES.encrypt(
        utf8EncodedToken,
        "secret_key"
    ).toString();

    const encodedEncryptedToken = encodeURIComponent(encryptedToken);
    const linkVar = `${QUIZMASTER_SESSION_WEBSITE}?name=${username}&token=${encodedEncryptedToken}`;
    return linkVar;
}
