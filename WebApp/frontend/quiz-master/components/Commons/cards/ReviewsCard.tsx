"use client";

import { Button, Divider, Text } from "@mantine/core";
import reviewLogo from "/public/review-logo.svg";
import starHalfFill from "/public/star-half-fill.svg";
import star from "/public/star.svg";
import starFill from "/public/star-fill.svg";
import Image from "next/image";
import ContactDetails from "@/components/Commons/modals/contactDetails";
import ContactUsForm from "@/components/Commons/form/ContactUsForm";
import { useEffect, useState } from "react";
import {
    fetchLoginUser,
    fetchReviewsForAdmin,
    fetchReviewsForClient,
} from "@/lib/quizData";
import { Review } from "@/lib/definitions";
import ReviewPostedCard from "./ReviewPostedCard";
import FeedbackModal from "../modals/FeedbackModal";

export default function ReviewsCard() {
    const [isAdmin, setIsAdmin] = useState<boolean>(false);
    const [reviews, setReviews] = useState<Review[]>([]);
    const [averageStar, setAverageStar] = useState<number>(0.0);
    const [openFeedback, setOpenFeedback] = useState<boolean>(false);

    useEffect(() => {
        fetchLoginUser().then((res) => {
            res?.info.roles.map((role) => {
                if (role === "Administrator") {
                    setIsAdmin(true);
                }
            });
        });
    }, []);

    useEffect(() => {
        if (isAdmin) {
            fetchReviewsForAdmin().then((res) => {
                setReviews(res);
            });
        } else {
            fetchReviewsForClient().then((res) => {
                setReviews(res);
            });
        }
    }, [isAdmin]);

    useEffect(() => {
        const stars = reviews.map((r) => r.starRating);
        console.log(stars);
        const averageRating =
            stars.length > 0
                ? +(
                      stars.reduce((acc, rating) => acc + rating, 0) /
                      stars.length
                  ).toFixed(1)
                : 0.0;

        setAverageStar(averageRating);
    }, [reviews]);

    return (
        <div className="flex md:flex-row flex-col text-black w-full gap-5 bg-white p-8 rounded-xl shadow-2xl">
            <div className="flex flex-col gap-2 flex-1 ">
                <p className=" font-bold text-3xl">Reviews</p>
                <p className="text-sm pt-3">
                    Read what our customers are saying about us. If you've
                    experienced our products or services, we'd love to hear your
                    feedback. Share your thoughts with us by filling out the
                    review form below.
                </p>
                <div>
                    <Image
                        src={reviewLogo}
                        alt="Contact Us"
                        width={500}
                        height={500}
                    />
                </div>
            </div>
            <div className="flex flex-col gap-2 flex-1 border-2 rounded-xl max-h-[500px] p-3">
                <div className="flex sm:flex-row flex-col justify-between gap-2">
                    <div>
                        <p className=" font-bold text-2xl">QuizMaster</p>
                        <p>Developed by: Full Scale / Gigabook</p>
                    </div>
                    <Button
                        variant="filled"
                        color="orange"
                        onClick={() => setOpenFeedback(true)}
                    >
                        Write a Review
                    </Button>
                </div>
                <div className="flex flex-row gap-2 items-baseline">
                    <p className="text-3xl text-[#F5B615]">{averageStar}</p>
                    {[0, 1, 2, 3, 4].map((index) => {
                        return (
                            <div key={index}>
                                {averageStar > index &&
                                averageStar < index + 1 ? (
                                    <Image
                                        src={starHalfFill}
                                        alt="Star"
                                        width={25}
                                        height={25}
                                    />
                                ) : averageStar > index ? (
                                    <Image
                                        src={starFill}
                                        alt="Star"
                                        width={25}
                                        height={25}
                                    />
                                ) : (
                                    <Image
                                        src={star}
                                        alt="Star"
                                        width={25}
                                        height={25}
                                    />
                                )}
                            </div>
                        );
                    })}
                    <p>{reviews.length} reviews</p>
                </div>
                <Divider />
                <div className="overflow-y-scroll">
                    {reviews.map((review) => (
                        <div className="py-3 px-2" key={review.id}>
                            <ReviewPostedCard review={review} />
                        </div>
                    ))}
                </div>
            </div>
            {openFeedback && (
                <FeedbackModal
                    onClose={() => setOpenFeedback(false)}
                    opened={openFeedback}
                />
            )}
        </div>
    );
}
