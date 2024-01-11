import SideNav from "@/components/Commons/navbars/sidenav";
import Page from "./page";
import PageHeader from "@/components/Commons/headers/PageHeader";

const Layout = () => {
    return (
        <div className="flex h-screen w-full">
            <SideNav />
            <div className="w-full h-screen flex flex-col bg-[#F8F9FA]">
                <PageHeader>Account Audit</PageHeader>
                <Page />
            </div>
        </div>
    );
};

export default Layout;
