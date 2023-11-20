import clsx from "clsx";
import { Button, Collapse, UnstyledButton } from "@mantine/core";
import { useState } from "react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { ChevronUpIcon } from "@heroicons/react/24/outline";

type GroupLink = {
  icon: React.FC<any>;
  label: string;
  href?: string;
  initiallyOpened?: boolean;
  links?: { label: string; href: string }[];
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
      key={link.label}
      href={link.href}
      className={clsx(
        "ml-5 flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-3 text-sm font-medium hover:bg-[--primary-200] justify-start px-3",
        {
          "text-[--primary]  hover:bg-[--primary-200]": pathname === link.href,
        },
      )}
    >
      <p className="block">{link.label}</p>
    </Link>
  ));

  return (
    <>
      <button
        className={clsx(
          "flex justify-between h-[48px] transition-all duration-300 items-center  rounded-md py-3 text-sm font-medium hover:bg-[--primary-200] px-3",
          {
            "bg-[--primary] text-white  hover:bg-[--primary]": links
              ?.map((link2) => link2.href)
              .includes(pathname),
          },
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
      <Collapse in={opened}>{items}</Collapse>
    </>
  );
}
