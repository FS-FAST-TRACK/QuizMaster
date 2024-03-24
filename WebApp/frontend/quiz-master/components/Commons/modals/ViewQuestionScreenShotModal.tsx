import { useEffect, useState } from "react";
import { Button, Modal } from "@mantine/core";
import Image from "next/image";
import { ArrowDownIcon } from "@heroicons/react/24/outline";

export default function ViewQuestionScreenShotModal({
    opened,
    onClose,
    screenshotLink,
    participantName,
}: {
    opened: boolean;
    onClose: () => void;
    screenshotLink: string;
    participantName: string;
}) {
    const [imageBlobUrl, setImageBlobUrl] = useState<string | null>(null);

    const downloadImage = () => {
        if (imageBlobUrl) {
            const a = document.createElement("a");
            a.href = imageBlobUrl;
            a.download = "screenshot.png";
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    };

    useEffect(() => {
        const fetchImage = async () => {
            try {
                const response = await fetch(screenshotLink, {
                    method: "GET",
                    headers: { "Content-Type": "application/json" },
                    credentials: "include",
                });
                const blob = await response.blob();
                const url = URL.createObjectURL(blob);
                setImageBlobUrl(url);
            } catch (error) {
                console.error("Error fetching image:", error);
            }
        };

        if (opened && screenshotLink) {
            fetchImage();
        }

        return () => {
            if (imageBlobUrl) {
                URL.revokeObjectURL(imageBlobUrl);
            }
        };
    }, [opened, screenshotLink]); // Re-run effect when modal opens or screenshot link changes

    return (
        <Modal
            opened={opened}
            onClose={onClose}
            size={"xl"}
            centered
            title={
                <div className="flex justify-between gap-4 pl-4 pt-4">
                    <div>
                        <p className="text-xl mb-2">
                            Screenshot on <b>{`${participantName}'s`}</b> answer
                        </p>
                    </div>
                </div>
            }
        >
            {imageBlobUrl ? (
                <div className="px-4 pb-4">
                    <div className="border border-gray-300 rounded-lg shadow-md">
                        <Image
                            alt="question_screenshot"
                            src={imageBlobUrl}
                            layout="responsive"
                            width={800}
                            height={600}
                            className="w-96 h-auto"
                        />
                    </div>
                    <div className="flex mt-8 w-full justify-end">
                        <Button
                            color="var(--primary)"
                            leftSection={
                                <ArrowDownIcon width={20} height={20} />
                            }
                            onClick={downloadImage}
                        >
                            Download Image
                        </Button>
                    </div>
                </div>
            ) : (
                <div className="w-full h-[600px] bg-gray-300 rounded-md animate-pulse"></div>
            )}
        </Modal>
    );
}
