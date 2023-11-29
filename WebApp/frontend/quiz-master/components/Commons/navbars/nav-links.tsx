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
    {
        key: "dashboard",
        label: "Dashboard",
        href: "/dashboard",
        icon: HomeIcon,
    },
    {
        key: "questions",
        label: "Questions",
        icon: QuestionMarkCircleIcon,
        links: [
            {
                key: "questions-display",
                label: "Questions",
                href: "/questions",
            },
            {
                key: "question-sets",
                label: "Question Set",
                href: "/question-sets",
            },
            {
                key: "question-categories",
                label: "Categories",
                href: "/categories",
            },
            {
                key: "question-difficulties",
                label: "Difficulty",
                href: "/difficulties",
            },
        ],
    },
    {
        key: "quiz-rooms",
        label: "Quiz Rooms",
        href: "/quiz-rooms",
        icon: CircleStackIcon,
    },
    {
        key: "reports",
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
                            key={link.key}
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
                            <p className="md:block hidden">{link.label}</p>
                        </Link>
                    );
                }
                return (
                    <GroupLink
                        key={link.key}
                        icon={link.icon}
                        href={link.href}
                        links={link.links}
                        label={link.label}
                    />
                );
            })}
        </>
    );
}
