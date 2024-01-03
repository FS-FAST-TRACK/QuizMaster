import { Button, Modal, Space } from "@mantine/core";
import { QuestionCategory, QuestionDifficulty } from "@/lib/definitions";
import Link from "next/link";
import { DifficultyCardBody } from "../cards/DifficultyCard";

export default function ViewDifficultyModal({
    difficulty,
    onClose,
    opened,
}: {
    difficulty?: QuestionDifficulty;
    opened: boolean;
    onClose: () => void;
}) {
    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            title={
                <div className="font-bold text-3xl text-center">
                    {difficulty?.qDifficultyDesc}
                </div>
            }
            size="lg"
        >
            <div className="space-y-8">
                <DifficultyCardBody difficulty={difficulty} />
                <div className="flex justify-end">
                    <Button
                        variant="transparent"
                        color="gray"
                        onClick={onClose}
                    >
                        Cancel
                    </Button>
                    <Link
                        href={`difficulties/edit/${difficulty?.id}`}
                        className="flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-1 text-sm font-medium hover:bg-red-200 justify-start px-3 bg-[--error] text-white "
                    >
                        Remove Difficulty
                    </Link>
                </div>
            </div>
        </Modal>
    );
}
