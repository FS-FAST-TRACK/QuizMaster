import { humanFileSize } from "@/lib/helpers";
import { fetchMedia } from "@/lib/quizData";
import { SpeakerWaveIcon } from "@heroicons/react/24/outline";
import { FileInput } from "@mantine/core";
import { Dispatch, SetStateAction, useEffect, useState } from "react";

export default function AudioInput({
    fileAudio,
    setFileAudio,
    qAudioId,
}: {
    fileAudio: File | null;
    setFileAudio: Dispatch<SetStateAction<File | null>>;
    qAudioId?: string;
}) {
    const [blob, setBlob] = useState<null | string>("");
    useEffect(() => {
        if (qAudioId && qAudioId?.length > 15) {
            fetchMedia(qAudioId).then((res) => setBlob(res ? res.data : null));
        }
    }, [qAudioId]);
    return (
        <>
            <label
                htmlFor="question-audio"
                className="w-[200px] flex gap-4 border text-[#706E6D] bg-[#D9D9D9] px-4 py-3 rounded text-sm cursor-pointer"
            >
                <SpeakerWaveIcon className="w-5" />
                Insert Audio
            </label>
            {blob && !fileAudio && <audio src={blob} controls />}
            {fileAudio && (
                <audio src={URL.createObjectURL(fileAudio)} controls />
            )}
            <div>
                <div>{fileAudio?.name}</div>
                <div>{humanFileSize(fileAudio?.size)}</div>
            </div>
            <FileInput
                id="question-audio"
                className="hidden"
                accept="audio/*"
                value={fileAudio}
                onChange={setFileAudio}
            />
        </>
    );
}
