import SideNav from "@/components/Commons/navbars/sidenav";
import { useState } from "react";

export default function Layout({ children }: { children: React.ReactNode }) {
    const [isSideBarOpen, setIsSideBarOpen] = useState(true);
    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <div className="flex-none transition w-64 ">
                <SideNav />
            </div>
            <div className="flex-grow p-6 md:overflow-y-auto md:p-10">
                {children}
            </div>
        </div>
    );
}
