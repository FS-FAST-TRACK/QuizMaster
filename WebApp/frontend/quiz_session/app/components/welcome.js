"use client";

import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import { useEffect } from "react";
import { ConnectToHub } from "../util/Connections";
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

export default function Welcome() {
  const { connection, setConnection } = useConnection();
  const { setParticipants } = useParticipants();
  const { setConnectionId } = useConnectionId();
  const { setStart } = useStart();
  const { setChat } = useChat();
  const { setQuestion } = useQuestion();
  const { setLeaderboard } = useLeaderboard();
  const { setMetadata } = useMetaData();

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
        })
        .catch((err) => {
          console.error("Error starting connection:", err);
        });
    }
  }, [connection]);

  return (
    <>
      {/* <div className="bg-white w-1/4 h-1/4 rounded-lg">
        <Image
          src={logo1}
          alt="Quis Master Logo"
          className="w-full h-full p-5"
        />
      </div> */}
    </>
  );
}
