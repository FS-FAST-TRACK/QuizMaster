import Link from "next/link";
import Game from "@/app/dashboard/game";
import { ArrowLeftOnRectangleIcon } from "@heroicons/react/24/outline";

export default function PageHeaderQuizRoom({
    children,
}: {
    children: React.ReactNode;
}) {
    const joinRoomLink = Game();
    return (
        <div className="text-xl items-center flex-col font-bold text-white bg-gradient-to-r from-[#17A14B] to-[#1AC059] px-10 py-8 flex flex-col md:flex-row justify-between ">
          
            {children}
          
                     <Link
                        href={joinRoomLink}
                        className="mr-10 flex h-[40px] bg-[#FF7F2A] hover:bg-[#FFAD33] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                    >
                       <ArrowLeftOnRectangleIcon className="w-6" />
                        <p className="block pr-3">Join Room</p>
                    </Link>
   
        </div>
    );
}
