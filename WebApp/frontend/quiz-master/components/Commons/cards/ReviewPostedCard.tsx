import { Review } from "@/lib/definitions";
import star from "/public/star.svg";
import starFill from "/public/star-fill.svg";
import Image from "next/image";

export default function ReviewPostedCard({ review }: { review: Review }) {
    const rates: number[] = [1, 2, 3, 4, 5];

    return (
        <div className="border-2 rounded-xl p-2">
            <div className="flex flex-row w-full gap-2 items-center ">
                {rates.map((rate) => {
                    return (
                        <div key={rate}>
                            {rate > review.starRating ? (
                                <Image
                                    src={star}
                                    alt="Star"
                                    width={25}
                                    height={25}
                                />
                            ) : (
                                <Image
                                    src={starFill}
                                    alt="Star"
                                    width={25}
                                    height={25}
                                />
                            )}
                        </div>
                    );
                })}
            </div>
            <p>{review.comment}</p>
        </div>
    );
}
