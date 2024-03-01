import quizMasterIcon from "/public/quiz-master-icon-white.svg";
import Image from "next/image";
import SystemInfoCard from "@/components/Commons/cards/SystemInfoCard";
import ReviewsCard from "@/components/Commons/cards/ReviewsCard";
export default async function SystemInfo() {
    return (
        <>
            <div className="flex flex-row gap-5">
                <div className="flex flex-col lg:w-2/3 gap-2">
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
        </>
    );
}
