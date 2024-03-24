"use client";

import React from "react";
import Leaderboard from "./leaderboard";
import { useEffect, useState } from "react";
import { BASE_URL } from "@/app/util/api";
import useUserTokenData from "@/app/util/useUserTokenData";
import { useMetaData, useQuestion } from "@/app/util/store";
import { AnimateSlideInFromBottom } from "./AnimateSlideInFromBottom";
import ConfirmEndGameModal from "../modals/ConfirmEndGameModal";

export default function Interval({ leaderBoard, handleCloseLeaderboard }) {
  const { isAdmin } = useUserTokenData();
  const { question } = useQuestion();
  const { metadata } = useMetaData();
  const [endGameModal, setEndGameModal] = useState(false);

  const handleNextRound = () => {
    if (handleCloseLeaderboard) {
      const roomInformationJson = localStorage.getItem("_rI");
      const roomInformation = JSON.parse(roomInformationJson);

      if (roomInformation && roomInformationJson) {
        const token = localStorage.getItem("token");
        fetch(
          BASE_URL + `/gateway/api/room/proceed/${roomInformation.id}`,
          { headers: { Authorization: `Bearer ${token}` } },
          { credentials: "include" }
        ).catch((e) => {
          alert("Error: ", e);
        });
        handleCloseLeaderboard();
      }
    }
  };

  const handleForceExit = () => {
    const roomInformationJson = localStorage.getItem("_rI");
    const roomInformation = JSON.parse(roomInformationJson);

    if (roomInformation && roomInformationJson) {
      const token = localStorage.getItem("token");
      fetch(
        BASE_URL + `/gateway/api/room/forceExit/${roomInformation.id}`,
        { headers: { Authorization: `Bearer ${token}` } },
        { credentials: "include" }
      ).catch((e) => {
        alert("Error: ", e);
      });
      handleCloseLeaderboard();
    }

    // Always call
    handleNextRound();
  };
  useEffect(() => {
    // const timer = setInterval(() => {
    //   setSeconds((prevSeconds) => prevSeconds - 1);

    //   if (seconds === 0) {
    //     clearInterval(timer);
    //     setShowNextBtn(true);
    //   }
    // }, 1000);

    // return () => clearInterval(timer);
    if (question || metadata) {
      if (question.question) {
        if (question.remainingTime !== 0) {
          handleCloseLeaderboard();
        }
      }
    }
  }, [/*seconds,*/ question, metadata, handleCloseLeaderboard]);
  return (
    <>
      <div className="text-white flex items-center space-x-1 mt-4 flex-col">
        {/* <div>Next round in: </div>
        <div
          className={`${
            seconds <= 5 ? "text-red-500" : "text-white "
          } font-bold  text-4xl`}
        >
          {seconds}
        </div> */}
        <ConfirmEndGameModal
          opened={endGameModal}
          onClose={() => setEndGameModal(false)}
          onConfirm={handleForceExit}
        />
      </div>
      <Leaderboard leaderBoard={leaderBoard} />
      {isAdmin && (
        <div className="w-full bg-gradient-to-t from-green-800 to-green-600/50 h-32 flex gap-4 justify-center items-center ">
          <button
            onClick={() => {
              setEndGameModal(true);
            }}
            className="border-2 border-white text-white px-4 py-2 rounded-md hover:bg-red-600 hover:border-red-600 hover:-translate-y-1 hover:scale-110 transition ease-in-out"
          >
            End Game
          </button>
          <button
            onClick={handleNextRound}
            className="text-white bg-orange-500 px-4 py-2 rounded-md hover:-translate-y-1 hover:scale-110 transition ease-in-out"
          >
            Next Round
          </button>
        </div>
      )}
    </>
  );
}
