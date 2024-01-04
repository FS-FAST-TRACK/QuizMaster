"use client";

import React from "react";
import { useEffect } from "react";
import { useConnection } from "@/app/util/store";
import { useRouter, usePathname, useSearchParams } from "next/navigation";
import { Button } from "@mantine/core";

export default function Start() {
  const { connection } = useConnection();
  const { push } = useRouter();
  const pathName = usePathname();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  useEffect(() => {
    connection.on("start", (isStart) => {
      console.log(isStart);
      if (isStart) {
        params.set("roomPin", params.get("roomPin"));
        push(`${pathName}/quiz?${params.toString()}`);
      }
    });
  }, []);

  const startQuiz = () => {
    connection.invoke("StartRoom", params.get("roomPin"));
  };
  return (
    <div className="flex bottom-0 h-20 items-center justify-center">
      <Button variant="filled" color="yellow" onClick={startQuiz}>
        Start Quiz
      </Button>
    </div>
  );
}
