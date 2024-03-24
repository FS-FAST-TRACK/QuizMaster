import React from "react";
import { Modal } from "@mantine/core";
import Image from "next/image";

export default function ImageModal({ opened, close, imageUrl }) {
  return (
    <Modal
      opened={opened}
      onClose={close}
      withCloseButton={false}
      centered
      size="xl"
    >
      <Image
        src={imageUrl}
        width={0}
        height={0}
        style={{ width: "100%", height: "100%" }}
        alt={imageUrl}
      />
    </Modal>
  );
}
