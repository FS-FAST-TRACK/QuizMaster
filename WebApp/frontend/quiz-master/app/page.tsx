import LoginForm from "@/components/login-form";
import { getServerSession } from "next-auth";
import HeadNav from "@/components/Commons/navbars/head-nav";
export default async function Home() {
    const session = await getServerSession();
    return (
        <main className="flex flex-col w-full h-full text-[#3C3C3C] bg-gradient-to-r from-[#17A14B] to-[#1AC159] gap-20 overflow-auto">
           {session ? (
                <div className="text-sm text-stone-500 flex flex-col">
                   <HeadNav  />
                 
                </div>
            ) : (
                <LoginForm callbackUrl="/dashboard" />
            )}
        </main>
    );
}
