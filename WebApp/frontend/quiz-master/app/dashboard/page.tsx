import { useSession } from "next-auth/react";
import Game from "./game";

export function generateMetadata() {
    return {
        title: "Dashboard",
    };
}

export default function Page() {
    return (
        <div>
            <div>Dashboard page</div>
            <Game />
        </div>
    );
}
