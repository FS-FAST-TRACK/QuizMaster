import quizMasterIcon from "/public/quiz-master-icon-white.svg";
import Image from "next/image";
import SystemInfoCard from "@/components/Commons/cards/SystemInfoCard";
import ReviewsCard from "@/components/Commons/cards/ReviewsCard";
import { Divider } from "@mantine/core";
export default async function SystemInfo() {
    return (
        <>
        <div className="py-11 px-23 ">
            <div  className="mb-10 -mt-12  flex flex-row gap-5 py-12 px-28  bg-black bg-opacity-30 text-white  rounded-lg">
              
                <div className="flex flex-col lg:w-2/3 gap-2 ">
                    <SystemInfoCard />
                </div>

                <div className="hidden lg:block xl:block w-1/3 relative h-[23rem]">
                    <Image
                        src={quizMasterIcon}
                        alt="QuizMaster Icon"
                        layout="fill"
                        objectFit="cover"
                    />
                </div>
            </div>
           
            <ReviewsCard />
            </div>
        </>
    );
}
