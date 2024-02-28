import React from "react";
import Image from "next/image";
import lifo from "@/public/test/lifo.jpg";
import { Button, Modal } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";

export default function MultipleChoiceAudio() {
  const [opened, { open, close }] = useDisclosure(false);
  return (
    <div className="w-full flex flex-col h-full">
      <Modal
        opened={opened}
        onClose={close}
        withCloseButton={false}
        centered
        size="xl"
      >
        <Image src={lifo} alt="LIFO diagram 1" />
      </Modal>
      <div className="flex flex-col items-center  flex-grow">
        <div className="text-white">Multiple Choice</div>
        <div className="text-white text-2xl font-bold flex flex-wrap text-center ">
          Which data structure is typically used to implement a
          Last-In-First-Out (LIFO) behavior
        </div>
        <div className="h-1/4 w-1/4 rounded-md" onClick={open}>
          <Image src={lifo} alt="LIFO diagram 2" />
        </div>
      </div>

      <div className="w-full grid grid-cols-2 place-content-center ">
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Queue
        </div>
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Linked List
        </div>
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Tree
        </div>
        <div className=" bg-white flex justify-center items-center m-5 text-xl font-bold p-3 text-green_text shadow-lg">
          Stack
        </div>
      </div>
      <div className=" w-full justify-center flex">
        <div className=" w-1/2 flex justify-center text-white text-2xl font-bold rounded-lg">
          <Button fullWidth color={"yellow"}>
            Submit
          </Button>
        </div>
      </div>
    </div>
  );
}
