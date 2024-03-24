import React from "react";
import Image from "next/image";
import logo1 from "@/public/logo/Logo1.svg";
import Login from "./componts/Login";
import Link from "next/link";
import { ADMIN_URL } from "../util/api";

export default function page() {
  return (
    <div className="w-96 h-fit bg-white flex flex-col items-center px-3 py-6 rounded-xl absolute top-10 2xl:top-20 shadow-lg">
      <Image src={logo1} className="mt-4 w-28" alt="Quiz Master Logo" />
      <div className="w-full flex items-center flex-col space-y-2 grow  justify-center mt-8 mb-8">
        <div className="text-xl font-bold">Sign Up</div>
        <div className="text-center text-secondary_text text-sm max-w-xs">
          Partial Sign-Up to create your account and enter the quiz room
        </div>
      </div>
      <div className=" space-y-2">
        <Login />
      </div>
      <div className="flex-row flex space-x-1 mt-4 mb-4 text-sm">
        <div className="text-gray-600">Already have an account?</div>
        <Link href={`${ADMIN_URL}/auth/login`} className="text-blue-500">
          {" "}
          Login
        </Link>
      </div>
    </div>
  );
}
