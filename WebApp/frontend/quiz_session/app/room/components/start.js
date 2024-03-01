"use client";

import React from "react";
import { useEffect } from "react";
import { useConnection, useStart } from "@/app/util/store";
import { useRouter, usePathname, useSearchParams } from "next/navigation";
import { Button } from "@mantine/core";
import { startQuiz } from "@/app/auth/util/handlers";
import { userInfo } from "@/app/util/api";
import useUserTokenData from "@/app/util/useUserTokenData";

export default function Start() {
  const { connection } = useConnection();
  const { isStart } = useStart();
  const { push } = useRouter();
  const pathName = usePathname();
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);

  const { isAdmin } = useUserTokenData();

  useEffect(() => {
    if (isStart) {
      params.set("roomPin", params.get("roomPin"));
      push(`${pathName}/quiz?${params.toString()}`);
    }
  }, [isStart]);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
    }
  }, []);

  return (
    <div className="flex bottom-0 h-20 items-center justify-center">
      <Button
        variant="filled"
        color="yellow"
        className="shadow-lg transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 hover:bg-yellow-500 duration-200"
        onClick={() => startQuiz(connection, params)}
        style={{ opacity: isAdmin ? 1 : 0 }}
        size="lg"
      >
        Start Quiz
      </Button>
    </div>
  );
}
