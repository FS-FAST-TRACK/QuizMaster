import { Button, Chip, Modal, Text } from "@mantine/core";
import { useEffect, useState } from "react";
import { Question, Set } from "@/lib/definitions";
import Link from "next/link";
import QuestionDetails from "../QuestionDetailsView";
import { fetchMedia } from "@/lib/quizData";
import Image from "next/image";
import QuesitonCard from "../cards/QuestionCard";
import SetCard from "../cards/SetCard";
import PromptModal from "./PromptModal";

export default function ViewSetModal({
    set,
    onClose,
    opened,
}: {
    set?: Set;
    opened: boolean;
    onClose: () => void;
}) {
    const [imageBlobUrl, setImageBlobUrl] = useState<null | string>(null);
    const [audioBlobUrl, setAudioBlobUrl] = useState<null | string>(null);

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            title="Set"
        >
            <div className="space-y-8 pl-3">
                <Text size="xl" fw={700}>
                    Set details:
                </Text>
                <SetCard set={set} />
            </div>
            <div className="flex justify-end">
                <Button variant="transparent" color="gray" onClick={onClose}>
                    Cancel
                </Button>
                <Link
                    href={`question-sets/edit/${set?.id}`}
                    className="flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-1 text-sm font-medium hover:bg-[--primary-200] justify-start px-3 bg-[--primary] text-white "
                >
                    Edit Set
                </Link>
            </div>
        </Modal>
    );

    // return (
    //     <PromptModal
    //         body={
    //             <div className="p-3">
    //                 <Text size="xl" fw={700}>
    //                     Set Details:
    //                 </Text>
    //                 <br />
    //                 <SetCard set={set} />
    //             </div>
    //         }
    //         action="Update"
    //         onConfirm={() => null}
    //         opened={set ? true : false}
    //         onClose={() => {}}
    //         title="Set"
    //     />
    // );
}
