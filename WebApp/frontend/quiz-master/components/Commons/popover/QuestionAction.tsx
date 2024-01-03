import {
    EllipsisVerticalIcon,
    PencilIcon,
    TrashIcon,
} from "@heroicons/react/24/outline";
import { Button, Popover } from "@mantine/core";
import Link from "next/link";
import { RefCallback } from "react";

export default function QuesitonAction({
    questionId,
    onDelete,
}: {
    questionId: number;
    onDelete: () => void;
}) {
    return (
        <Popover width={140} zIndex={10} position="bottom">
            <Popover.Target>
                <div className="cursor-pointer flex items-center justify-center aspect-square">
                    <EllipsisVerticalIcon className="w-6" />
                </div>
            </Popover.Target>
            <Popover.Dropdown p={10} className="space-y-3">
                <button
                    className="flex w-full p-2 gap-2 text-[var(--error)] rounded-lg hover:text-white hover:bg-[var(--error)]   "
                    onClick={onDelete}
                >
                    <TrashIcon className="w-6" />
                    <div>Remove</div>
                </button>
                <Link
                    href={`questions/edit/${questionId}`}
                    className="flex gap-2 p-2 text-[var(--success)] rounded-lg hover:text-white hover:bg-[var(--success)] "
                >
                    <PencilIcon className="w-6 " />
                    <div>Edit</div>
                </Link>
            </Popover.Dropdown>
        </Popover>
    );
}
