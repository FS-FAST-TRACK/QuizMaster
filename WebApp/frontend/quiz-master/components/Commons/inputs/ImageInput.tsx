import { humanFileSize } from "@/lib/helpers";
import { fetchMedia } from "@/lib/quizData";
import { PhotoIcon } from "@heroicons/react/24/outline";
import { FileInput } from "@mantine/core";
import Image from "next/image";
import { Dispatch, SetStateAction, useEffect, useState } from "react";

export default function ImageInput({
    fileImage,
    setFileImage,
    qImageId,
}: {
    fileImage: File | undefined | null;
    setFileImage: (file: File | null) => void;
    qImageId?: string;
}) {
    const [blob, setBlob] = useState<null | string>("");
    useEffect(() => {
        if (qImageId && qImageId?.length > 15) {
            fetchMedia(qImageId).then((res) => setBlob(res ? res.data : null));
        }
    }, [qImageId]);
    return (
        <>
            <label
                htmlFor="question-image"
                className="w-[200px] flex gap-4 border text-[#706E6D] bg-[#D9D9D9] px-4 py-3 rounded text-sm cursor-pointer"
            >
                <PhotoIcon className="w-5" />
                <p>Insert Image</p>
            </label>
            {blob && !fileImage && (
                <Image
                    alt="Image input"
                    src={blob}
                    width={100}
                    height={100}
                    className="w-fit h-72 aspect-auto object-contain"
                />
            )}

            {fileImage && (
                <Image
                    alt="Image input"
                    src={URL.createObjectURL(fileImage)}
                    width={100}
                    height={100}
                    className="w-fit h-72 aspect-auto object-contain"
                />
            )}

            <div>
                <div>{fileImage?.name}</div>
                <div>{humanFileSize(fileImage?.size)}</div>
            </div>
            <FileInput
                id="question-image"
                accept="image/png,image/jpeg"
                className="hidden"
                value={fileImage}
                onChange={setFileImage}
            />
        </>
    );
}
