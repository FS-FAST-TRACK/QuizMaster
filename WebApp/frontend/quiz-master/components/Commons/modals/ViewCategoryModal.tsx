import { Button, Modal, Space } from "@mantine/core";
import { QuestionCategory } from "@/lib/definitions";
import Link from "next/link";
import { CategoryBody } from "../cards/CategoryCard";

export default function ViewCategoryModal({
    category,
    onClose,
    opened,
}: {
    category?: QuestionCategory;
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
                    {category?.qCategoryDesc}
                </div>
            }
            size="lg"
        >
            <div className="space-y-8">
                <CategoryBody category={category} />

                <div className="flex justify-end">
                    <Button
                        variant="transparent"
                        color="gray"
                        onClick={onClose}
                    >
                        Cancel
                    </Button>
                    <Link
                        href={`categories/edit/${category?.id}`}
                        className="flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-1 text-sm font-medium hover:bg-[--primary-200] justify-start px-3 bg-[--primary] text-white "
                    >
                        Edit Category
                    </Link>
                </div>
            </div>
        </Modal>
    );
}
