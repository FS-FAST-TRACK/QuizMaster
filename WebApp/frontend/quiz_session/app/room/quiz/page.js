"use client";

import React, { useEffect, useState } from "react";
import Question from "./components/question";
import { SoundEffectsContext } from "../contexts/SoundEffectsContext";

export default function page() {
  const [volume, setVolume] = useState(100);
  const [isMute, setIsMute] = useState(false);

  useEffect(() => {
    if (volume === 0) {
      setIsMute(true);
    } else {
      setIsMute(false);
    }
  }, [volume]);

  return (
    <SoundEffectsContext.Provider
      value={{ volume, setVolume, isMute, setIsMute }}
    >
      <div className="h-full  flex flex-col">
        <Question />
      </div>
    </SoundEffectsContext.Provider>
  );
}
