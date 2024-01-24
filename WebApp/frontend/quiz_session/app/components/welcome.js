"use client";

import React from "react";
import { useEffect } from "react";
import {
  useConnection,
  useParticipants,
  useConnectionId,
  useChat,
  useStart,
  useQuestion,
  useLeaderboard,
  useMetaData,
} from "../util/store";
import { notifications } from "@mantine/notifications";
import { useSearchParams } from "next/navigation";
import { useRouter } from "next/navigation";

export default function Welcome() {
  const searchParams = useSearchParams();
  const params = new URLSearchParams(searchParams);
  const { connection, setConnection } = useConnection();
  const { setParticipants } = useParticipants();
  const { setConnectionId } = useConnectionId();
  const { setStart } = useStart();
  const { setChat } = useChat();
  const { setQuestion } = useQuestion();
  const { setLeaderboard } = useLeaderboard();
  const { setMetadata } = useMetaData();
  const { push } = useRouter();

  useEffect(() => {
    setConnection();
  }, []);
  useEffect(() => {
    if (connection !== undefined && connection.state !== "Connected") {
      connection
        .start()
        .then(() => {
          //notif
          connection.on("notif", (message) => {
            notifications.show({
              title: message,
            });
          });

          // //chat
          connection.on("chat", (message) => {
            setChat(message);
          });

          //participants
          connection.on("participants", (players) => {
            setParticipants(players);
          });

          //connection ID
          connection.on("connId", (connId) => {
            setConnectionId(connId);
          });

          //start
          connection.on("start", (isStart) => {
            setStart(isStart);
          });

          //question
          connection.on("question", (question) => {
            setQuestion(question);
          });

          //leaderboard
          connection.on("leaderboard", (leader, isStop) => {
            setLeaderboard({ scores: leader, stop: isStop });
          });

          //metadata
          connection.on("metadata", (metadata) => {
            console.log(metadata);
            setMetadata(metadata);
          });

          const token = params.get("token");
          const username = params.get("name");

          if (token && username && connection.state === "Connected") {
            connection.invoke("Login", token);
            localStorage.setItem("username", username.toLowerCase());
            push("/auth/code");
          } else {
            push("/auth");
          }
        })
        .catch((err) => {
          console.error("Error starting connection:", err);
        });
    }
  }, [connection]);

  return (
    // <div>
    //   {/* <div className="bg-white w-1/4 h-1/4 rounded-lg">
    //     <Image
    //       src={logo1}
    //       alt="Quis Master Logo"
    //       className="w-full h-full p-5"
    //     />
    //   </div> */}
    //   <div>{params.get("name")}</div>
    //   <div>{params.get("token")}</div>
    // </div>
    <></>
  );
}
