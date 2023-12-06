import { Button, Modal, Space } from "@mantine/core";
import { QuestionCategory, QuestionDifficulty } from "@/lib/definitions";
import Link from "next/link";

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
                <div className="flex flex-col gap-5 md:flex-row md:[&>*]:basis-1/3 ">
                    <div>
                        <p>Date Created</p>
                        <p className="text-xl font-bold">
                            {difficulty?.dateCreated.toDateString()}
                        </p>
                    </div>
                    <div>
                        <p>Last Modified</p>
                        <p className="text-xl font-bold">
                            {difficulty?.dateUpdated.toDateString()}
                        </p>
                    </div>
                    <div>
                        <p>Questions</p>
                        <p className="text-xl font-bold">
                            {difficulty?.questionCounts}
                        </p>
                    </div>
                </div>

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
