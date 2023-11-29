import clsx from "clsx";
import { Button, Collapse, Popover, UnstyledButton } from "@mantine/core";
import { useState } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { ChevronUpIcon } from "@heroicons/react/24/outline";

type GroupLink = {
    icon: React.FC<any>;
    label: string;
    href?: string;
    initiallyOpened?: boolean;
    links?: { label: string; href: string; key: string }[];
};

export default function GroupLink({
    icon: Icon,
    label,
    initiallyOpened,
    links,
}: GroupLink) {
    const pathname = usePathname();

    const hasLinks = Array.isArray(links);
    const [opened, setOpened] = useState(initiallyOpened || false);

    const items = (hasLinks ? links : []).map((link) => (
        <Link
            key={link.key}
            href={link.href}
            className={clsx(
                "ml-5 flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-3 text-sm font-medium hover:bg-[--primary-200] justify-start px-3",
                {
                    "text-[--primary]  hover:bg-[--primary-200]":
                        pathname.includes(link.href),
                }
            )}
        >
            <p className="block">{link.label}</p>
        </Link>
    ));

    return (
        <>
            <div className="md:hidden">
                <Popover
                    width={300}
                    trapFocus
                    position="bottom"
                    withArrow
                    shadow="md"
                >
                    <Popover.Target>
                        <button
                            className={clsx(
                                "justify-between h-[48px] transition-all duration-300 items-center  rounded-md py-3 text-sm font-medium hover:bg-[--primary-200] px-3",
                                {
                                    "bg-[--primary] text-white  hover:bg-[--primary]":
                                        links?.findIndex((link) =>
                                            pathname.includes(link.href)
                                        ) != -1,
                                }
                            )}
                        >
                            <Icon className="w-6" />
                        </button>
                    </Popover.Target>
                    <Popover.Dropdown>{items}</Popover.Dropdown>
                </Popover>
            </div>
            <button
                className={clsx(
                    "hidden md:flex justify-between h-[48px] transition-all duration-300 items-center  rounded-md py-3 text-sm font-medium hover:bg-[--primary-200] px-3",
                    {
                        "bg-[--primary] text-white  hover:bg-[--primary]":
                            links?.findIndex((link) =>
                                pathname.includes(link.href)
                            ) != -1,
                    }
                )}
                onClick={() => setOpened(!opened)}
            >
                <div className="flex gap-3 items-center">
                    <Icon className="w-6" />
                    {label}
                </div>
                {hasLinks && (
                    <div>
                        <ChevronUpIcon
                            className="text-whitem w-6 transition-all duration-500"
                            style={{
                                transform: opened ? "none" : "rotateX(180deg)",
                            }}
                        />
                    </div>
                )}
            </button>
            <Collapse className="hidden md:block" in={opened}>
                {items}
            </Collapse>
        </>
    );
}
