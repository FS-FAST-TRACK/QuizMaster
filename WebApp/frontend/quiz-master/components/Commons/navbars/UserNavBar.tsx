import {
    ArrowRightCircleIcon,
    ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/outline";
import { Button } from "@mantine/core";
import { useSession } from "next-auth/react";
import Link from "next/link";

export default function UserNavBar() {
    const { data: session } = useSession();
    const user = session?.user;
    return (
        <div className="flex border-t">
            <div className="grow transition-all duration-300 items-center py-3 text-sm font-medium hover:bg-[--primary-200] px-3">
                <Link href="/profile">
                    <div className="text-sm font-medium">{user?.username}</div>
                    <div className="text-xs text-gray-500 ">{user?.email}</div>
                </Link>
            </div>
            <Link href="/auth/signout" className="hover:bg-[--primary]">
                <ArrowRightOnRectangleIcon className="h-full w-auto aspect-square py-4" />
            </Link>
        </div>
    );
}
