import SideNav from "@/components/Commons/navbars/sidenav";

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <div className="flex-none transition w-12 md:w-64 ">
                <SideNav />
            </div>
            <div className="flex-grow p-6 md:overflow-y-auto md:p-10">
                {children}
            </div>
        </div>
    );
}
