import { useSession } from "next-auth/react";
import PageHeader from "@/components/Commons/headers/PageHeader";
import SideNav from "@/components/Commons/navbars/sidenav";
export function generateMetadata() {
    return {
        title: "Dashboard",
    };
}

export default function Page() {
    return (
        <div >
            Hello
        </div>
    );
}
