import HeadNav from "@/components/Commons/navbars/head-nav";

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col w-full h-full py-8 px-24 text-white bg-gradient-to-r from-[#17A14B] to-[#1AC159] gap-16 overflow-auto">
            <HeadNav />
            {children}
            <div>
                <p className="text-sm text-white">
                    Copyright 2023 Ⓒ QuizMaster
                </p>
            </div>
        </div>
    );
}
