import Link from "next/link";
import NavLinks from "@/components/Commons/navbars/nav-links";
import { PowerIcon } from "@heroicons/react/24/outline";
import Image from "next/image";
import logo from "/public/quiz-master-logo.png";

export default function SideNav() {
  return (
    <div className="p-5 h-full">
      <div className="flex h-full flex-col px-3 py-4 md:px-2 bg-green-700 rounded-3xl">
        <Link
          className="mb-2 flex h-20 items-end justify-start rounded-md p-4 md:h-20"
          href="/"
        >
          <div className="w-32 text-white md:w-40">
            <Image
              className=""
              src={logo}
              alt="QuizMaster Logo"
              width={100}
              height={100}
            />
          </div>
        </Link>
        <div className="flex grow flex-row justify-between space-x-2 md:flex-col md:space-x-0 md:space-y-2">
          <NavLinks />
          <div className="hidden h-auto w-full grow rounded-md bg-transparent md:block"></div>
          <form>
            <button className="flex w-full h-[48px] grow items-center justify-center gap-2 rounded-md bg-gray-50 p-3 text-sm font-medium hover:bg-sky-100 hover:text-blue-600 md:flex-none md:justify-start md:p-2 md:px-3">
              <PowerIcon className="w-6" />
              <div className="hidden md:block">Sign Out</div>
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
