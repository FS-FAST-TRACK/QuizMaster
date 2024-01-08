"use client";

import React from "react";
import { useEffect } from "react";
import { useConnection, useStart } from "@/app/util/store";
import { useRouter, usePathname, useSearchParams } from "next/navigation";
import { Button } from "@mantine/core";
import { startQuiz } from "@/app/auth/util/handlers";

export default function Start() {
  const { connection } = useConnection();
  const { isStart } = useStart();
  const { push } = useRouter();
  const pathName = usePathname();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  useEffect(() => {
    if (isStart) {
      params.set("roomPin", params.get("roomPin"));
      push(`${pathName}/quiz?${params.toString()}`);
    }
  }, [isStart]);

  return (
    <div className="flex bottom-0 h-20 items-center justify-center">
      <Button
        variant="filled"
        color="yellow"
        onClick={() => startQuiz(connection, params)}
      >
        Start Quiz
      </Button>
    </div>
  );
}
