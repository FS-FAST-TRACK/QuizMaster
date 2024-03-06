import Link from "next/link";
import { ArrowLongRightIcon } from "@heroicons/react/24/outline";

export default function PageHeaderDashboard({
    children,
}: {
    children: React.ReactNode;
}) {
 
    return (
        <div className="text-xl items-center flex-col font-bold text-white bg-gradient-to-r from-[#17A14B] to-[#1AC059] px-10 py-8 flex flex-col md:flex-row justify-between ">
          
            {children}
          
                     <Link
                        href={"/home"}
                        className="mr-4 flex h-[40px] bg-amber-500 hover:bg-[#FFAD33] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                    >
                        <p className="block pl-3">Go to Home</p>
                        <ArrowLongRightIcon className="w-5" />
                    </Link>
        </div>
    );
}
