import { Button, Chip, Modal } from "@mantine/core";
import { useEffect, useState } from "react";
import { Question } from "@/lib/definitions";
import Link from "next/link";
import QuestionDetails from "../QuestionDetailsView";
import { fetchMedia } from "@/lib/quizData";
import Image from "next/image";
import QuesitonCard from "../cards/QuestionCard";

export default function ViewQuestionModal({
    question,
    onClose,
    opened,
    callInQuestionsPage,
}: {
    question?: Question;
    opened: boolean;
    onClose: () => void;
    callInQuestionsPage: string;
}) {
    const [imageBlobUrl, setImageBlobUrl] = useState<null | string>(null);
    const [audioBlobUrl, setAudioBlobUrl] = useState<null | string>(null);
    useEffect(() => {
        if (question && question?.qImage.length > 15) {
            fetchMedia(question?.qImage).then((res) => {
                setImageBlobUrl(res ? res.data : null);
            });
        } else {
            setImageBlobUrl(null);
        }
        if (question && question?.qAudio.length > 15) {
            fetchMedia(question?.qAudio).then((res) =>
                setAudioBlobUrl(res ? res.data : null)
            );
        } else {
            setAudioBlobUrl(null);
        }
    }, [question?.qAudio, question?.qImage]);

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            size="lg"
        >
            <div className="space-y-8">
                <QuesitonCard question={question} />
                {imageBlobUrl && (
                    <div>
                        <p>Image</p>
                        <Image
                            alt="Image"
                            src={imageBlobUrl}
                            width={100}
                            height={100}
                            className="w-fit h-72 aspect-auto object-contain"
                        />
                    </div>
                )}
                {audioBlobUrl && (
                    <div>
                        <p>Audio</p>
                        <audio src={audioBlobUrl} controls />{" "}
                    </div>
                )}

                <QuestionDetails
                    questionId={question?.id}
                    questionTypeId={question?.qTypeId}
                />
                {callInQuestionsPage === "questions" && (
                    <div className="flex justify-end">
                        <Button
                            variant="transparent"
                            color="gray"
                            onClick={onClose}
                        >
                            Cancel
                        </Button>
                        <Link
                            href={`questions/edit/${question?.id}`}
                            className="flex h-[48px] transition-all duration-300 items-center gap-3 rounded-md py-1 text-sm font-medium hover:bg-[--primary-200] justify-start px-3 bg-[--primary] text-white "
                        >
                            Edit Question
                        </Link>
                    </div>
                )}
            </div>
        </Modal>
    );
}
