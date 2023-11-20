import LoginForm from "@/components/login-form";
import LogoutButton from "@/components/logout-button";
import { Button } from "@mantine/core";
import { getServerSession } from "next-auth";
import { signOut } from "next-auth/react";
import { cookies } from "next/headers";

export default async function Home() {
    const session = await getServerSession();

    return (
        <main className="flex min-h-screen flex-col items-center justify-center h-screen p-24 bg-gradient-to-tr from-30% from-[#17A14B] to-[#1BD260]">
            {session ? (
                <div className="text-sm text-stone-500 flex flex-col">
                    logged in as: @{session.user.name}
                    <LogoutButton />
                </div>
            ) : (
                <LoginForm />
            )}
        </main>
    );
}
