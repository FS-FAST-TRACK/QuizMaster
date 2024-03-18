import { Button, Modal, RingProgress } from "@mantine/core";
import React, { useState } from "react";
import { useEffect } from "react";
import { useRef } from "react";
import { useLongPress } from "use-long-press";

export default function ConfirmEndGameModal({
  opened = false,
  onClose = () => {},
  setsRemaining = 0,
  onConfirm = () => {},
}) {
  const [ringValue, setRingValue] = useState(0);
  const intervalRef = useRef(null);

  const bind = useLongPress(
    () => {
      onConfirm();
      clearInterval(intervalRef.current);
    },
    {
      onStart: (event) => {
        intervalRef.current = setInterval(() => {
          console.log("interval");
          setRingValue((prev) => prev + 7.5);
        }, 200);
      },
      onFinish: (event) => {
        clearInterval(intervalRef.current);
      },
      onCancel: (event) => {
        clearInterval(intervalRef.current);
        setRingValue(0);
      },
      threshold: 3000,
    }
  );

  useEffect(() => {
    if (!opened) {
      setRingValue(0);
    }
  }, [opened]);

  return (
    <Modal
      opened={opened}
      onClose={onClose}
      title={<p className="text-lg font-semibold">End this Quiz</p>}
    >
      <div>
        <p className="text-base mb-8">{`Are you sure want to end this quiz?`}</p>
        <div className="gap-2 flex flex-col">
          <Button
            {...bind()}
            variant="filled"
            color="red"
            className="bg-red-600"
            leftSection={
              <RingProgress
                thickness={4}
                sections={[{ value: ringValue, color: "white" }]}
                rootColor="rgb(220 38 38)"
                size={32}
              />
            }
          >
            Yes, end this Quiz
          </Button>
          <Button variant="outline" color="gray" onClick={() => onClose()}>
            Cancel
          </Button>
        </div>
      </div>
    </Modal>
  );
}
