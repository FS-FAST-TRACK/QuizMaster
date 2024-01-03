"use client";

import LoginForm from "@/components/login-form";
import RegisterForm from "@/components/register-form";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";

export default function Page({
    params,
}: {
    params: { authPage: "login" | "signup" | undefined; callbackUrl: string };
}) {
    const authPage = params.authPage || "login";
    const callbackUrl = params.callbackUrl || "/dashboard";
    const { status } = useSession();

    if (status === "loading") {
        return <div>loading</div>;
    }
    if (status === "authenticated") {
        redirect(callbackUrl);
    }

    return (
        <div className="flex min-h-screen flex-col items-center justify-center h-screen p-10 md:p-24 bg-gradient-to-tr from-30% from-[#17A14B] to-[#1BD260]">
            <div className={`${authPage === "login" ? "" : "hidden"}`}>
                <LoginForm callbackUrl={callbackUrl} />
            </div>
            <div
                className={`${
                    authPage === "signup" ? "" : "hidden"
                } w-full flex justify-center`}
            >
                <RegisterForm />
            </div>
        </div>
    );
}
