import { Transition } from "@headlessui/react";
import React, { useEffect, useState } from "react";

export const DURATIONS = {
  DELAY_0: "0",
  DELAY_75: "75",
  DELAY_100: "100",
  DELAY_150: "150",
  DELAY_200: "200",
  DELAY_300: "300",
  DELAY_500: "500",
  DELAY_700: "700",
  DELAY_1000: "1000",
};

export function AnimateSlideInFromBottom({
  children,
  delay = "500",
  className = "",
}) {
  const [isShowing, setIsShowing] = useState(false);

  useEffect(() => {
    setTimeout(() => {
      setIsShowing(true);
    }, 500);
  }, []);

  function getDuration(obj, value) {
    if (Object.values(obj).includes(value)) {
      return value;
    } else {
      if (parseInt(value)) {
        return `[${value}ms]`;
      } else {
        return `[300ms]`;
      }
    }
  }

  return (
    <Transition
      show={isShowing}
      enter={`transition ease-in-out duration-500 transform delay-${getDuration(
        DURATIONS,
        delay
      )}`}
      className={className}
      enterFrom="translate-y-full opacity-0 "
      enterTo="translate-y-0 opacity-100"
      leave="transition ease-in-out duration-300 transform"
      leaveFrom="translate-y-0 opacity-100"
      leaveTo="-translate-y-full opacity-0"
    >
      {children}
    </Transition>
  );
}
