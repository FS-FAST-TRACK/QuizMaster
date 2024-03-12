"use client";

import React, { useState } from "react";
import Chat from "./components/chat";
import Room from "./components/room";

export default function page() {
  const [collapsed, setCollapsed] = useState(false);

  const toggleChat = () => {
    setCollapsed((prev) => setCollapsed(!prev));
  };

  return (
    <div className="w-full h-full  flex flex-row relative">
      <div className="w-full sm:w-full md:w-2/3 lg:w-3/4">
        <Room onToggleCollapseChat={() => toggleChat()} />
      </div>
      <div
        className={`bg-white h-full w-full md:block md:w-1/3 lg:block lg:w-1/4 ${
          collapsed
            ? "absolute top-0 right-0 left-0 bottom-0 md:block md:static"
            : "hidden"
        }`}
      >
        <Chat onToggleCollapseChat={toggleChat} />
      </div>
    </div>
  );
}
