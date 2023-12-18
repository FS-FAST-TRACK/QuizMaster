"use client";
import NextAuth from "next-auth/next";
import { signOut, useSession } from "next-auth/react";
import { redirect } from "next/navigation";
import { useEffect } from "react";

export default function Page() {
    signOut();

    return <></>;
}
