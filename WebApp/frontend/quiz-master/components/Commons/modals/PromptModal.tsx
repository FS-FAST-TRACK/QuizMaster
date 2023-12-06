import { Box, Button, Modal } from "@mantine/core";
import { ReactNode } from "react";

export default function PromptModal({
    title,
    body,
    action,
    onConfirm,
    onClose,
    opened,
}: {
    title?: string;
    body?: ReactNode;
    action: string;
    onConfirm: () => void;
    opened: boolean;
    onClose: () => void;
}) {
    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            title={title}
            centered
        >
            {body}
            <div className="flex justify-end">
                <Button variant="transparent" color="gray" onClick={onClose}>
                    Cancel
                </Button>
                <Button variant="filled" color="red" onClick={onConfirm}>
                    {action}
                </Button>
            </div>
        </Modal>
    );
}
