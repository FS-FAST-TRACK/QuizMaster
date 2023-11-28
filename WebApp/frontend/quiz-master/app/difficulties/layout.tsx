import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex h-screen flex-row md:overflow-hidden">
            <div className="flex-none transition w-64 hidden md:block">
                <SideNav />
            </div>
            <div className="grow md:overflow-y-auto flex flex-col bg-[#F8F9FA]">
                <PageHeader>Difficulties</PageHeader>
                {children}
            </div>
        </div>
    );
}
