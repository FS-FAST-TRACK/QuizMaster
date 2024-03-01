"use client";

import React from "react";
import Leaderboard from "./leaderboard";
import { useEffect, useState } from "react";
import { BASE_URL } from "@/app/util/api";
import useUserTokenData from "@/app/util/useUserTokenData";
import { useMetaData, useQuestion } from "@/app/util/store";

export default function Interval({ leaderBoard, handleCloseLeaderboard}) {
  const { isAdmin } = useUserTokenData();
  const { question } = useQuestion();
  const { metadata } = useMetaData();
  

  const handleNextRound = () => {
    if(handleCloseLeaderboard){
      const roomInformationJson = localStorage.getItem("_rI");
      const roomInformation = JSON.parse(roomInformationJson);

      if(roomInformation && roomInformationJson){
        const token = localStorage.getItem("token")
        fetch(BASE_URL+`/gateway/api/room/proceed/${roomInformation.id}`, {headers:{"Authorization": `Bearer ${token}`}}, {credentials: "include"}).catch(e => {alert("Error: ",e)})
        handleCloseLeaderboard()
      }
      
    }
  }
  useEffect(() => {
    // const timer = setInterval(() => {
    //   setSeconds((prevSeconds) => prevSeconds - 1);

    //   if (seconds === 0) {
    //     clearInterval(timer);
    //     setShowNextBtn(true);
    //   }
    // }, 1000);

    // return () => clearInterval(timer);
    if(question || metadata){
      if(question.question){
        if(question.remainingTime !== 0){
          handleCloseLeaderboard()
        }
      }
    }
  }, [/*seconds,*/ question ,metadata, handleCloseLeaderboard]);
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
      </div>
      <Leaderboard leaderBoard={leaderBoard} />
      {isAdmin &&
      (
        <div className="w-full flex items-center b-[50px] h-[30%]">
          <button onClick={handleNextRound} className="text-white bg-orange-600 m-auto px-4 py-2 rounded-md">Next Round</button>
        </div>
      )}
    </>
  );
}
