
import LoginForm from "@/components/login-form";
import { getServerSession } from "next-auth";
import HeadNav from "@/components/Commons/navbars/head-nav";

import React from "react";
export default async function Home() {
    const session = await getServerSession();
    return (
        <main  >
           {session ? (
               <HeadNav  />
            ) : (
                <LoginForm callbackUrl="/" />
            )}
        </main>
    );
}
