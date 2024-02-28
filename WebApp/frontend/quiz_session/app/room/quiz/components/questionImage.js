import React from "react";
import image from "@/public/icons/image.png";
import Image from "next/image";

export default function QuestionImage({ imageUrl, open }) {
  return (
    <>
      {imageUrl ? (
        <Image
          src={imageUrl}
          width={0}
          height={0}
          sizes="100vw"
          style={{ width: "25%", height: "auto" }}
          onClick={open}
          className="rounded-md"
          alt={imageUrl}
        />
      ) : (
        <div className=" w-1/4 h-fit bg-dark_green flex justify-center items-center rounded-lg animate-pulse text-white p-2">
          <Image
            src={image}
            width={0}
            height={0}
            sizes="100vw"
            className="rounded-md"
            alt={imageUrl}
          />
        </div>
      )}
    </>
  );
}
