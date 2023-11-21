"use client";

import {
    HomeIcon,
    QuestionMarkCircleIcon,
    CircleStackIcon,
    DocumentTextIcon,
} from "@heroicons/react/24/outline";
import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from "clsx";
import GroupLink from "./GroupLink";

// Map of links to display in the side navigation.
// Depending on the size of the application, this would be stored in a database.
const links = [
    { label: "Dashboard", href: "/dashboard", icon: HomeIcon },
    {
        label: "Questions",
        icon: QuestionMarkCircleIcon,
        links: [
            {
                label: "Questions",
                href: "/questions",
            },
            {
                label: "Question Set",
                href: "/question-sets",
            },
            {
                label: "Categories",
                href: "/categories",
            },
            {
                label: "Difficulty",
                href: "/difficulties",
            },
        ],
    },
    {
        label: "Quiz Rooms",
        href: "/quiz-rooms",
        icon: CircleStackIcon,
    },
    {
        label: "Reports",
        href: "/reports",
        icon: DocumentTextIcon,
    },
];

export default function NavLinks() {
    const pathname = usePathname();

    return (
        <>
            {links.map((link) => {
                const LinkIcon = link.icon;
                if (link?.href) {
                    return (
                        <Link
                            key={link.label}
                            href={link.href}
                            className={clsx(
                                "flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-3 text-sm font-medium hover:bg-[--primary-200] justify-start p-2 px-3",
                                {
                                    "bg-[--primary] text-white  hover:bg-[--primary]":
                                        pathname === link.href,
                                }
                            )}
                        >
                            <LinkIcon className="w-6" />
                            <p className="block">{link.label}</p>
                        </Link>
                    );
                }
                return <GroupLink {...link} />;
            })}
        </>
    );
}
