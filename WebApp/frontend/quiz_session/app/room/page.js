import React from "react";
import Chat from "./components/chat";
import Room from "./components/room";

export default function page() {
  return (
    <div className="w-full h-full  flex flex-row">
      <div className="w-3/4">
        <Room />
      </div>
      <div className="w-1/4 h-full bg-white">
        <Chat />
      </div>
    </div>
  );
}
