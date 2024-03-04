import SideNav from "@/components/Commons/navbars/sidenav";
import PageHeaderDashboard from "@/components/Commons/headers/PageHeaderDashboard";
import Card from "./card";
export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex h-screen flex-row md:overflow-hidden ">
            <div className="flex-none transition w-12 md:w-64 ">
                <SideNav />
                
            </div>
            <div className="w-full  h-screen flex flex-col  relative">
            <PageHeaderDashboard>Dashboard</PageHeaderDashboard>
            <Card />
            </div>
        </div>
    );
}
