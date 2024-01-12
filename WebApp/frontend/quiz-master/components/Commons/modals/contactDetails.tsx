"use client";
import { useEffect, useState } from "react";
import Image from "next/image";
import emailIcon from "/public/email-icon.png";
import phoneIcon from "/public/telephone-icon.png";
import { getServerSession } from "next-auth";
import Link from "next/link";
import UpdateContactInfoModal from "./UpdateContactInfoModal";
import { ContactDetails } from "@/lib/definitions";

export default function ContactDetails({ email }: { email?: string }) {
    const [contactDetails, setContactDetails] = useState<ContactDetails>({
        email: "alyssa@gmail.com",
        phoneNumber: "09083547069",
    });
    const [openFeedback, setOpenFeedback] = useState<boolean>(false);

    return (
        <div className="mt-[-100px]">
            <div className=" bg-[#4F4F4F] w-fit p-2 opacity-90 text-white gap-2 rounded-lg">
                <div className="flex flex-row gap-3">
                    <p className=" font-thin text-xs items-center">
                        You can also reach us through the following:
                    </p>
                    {email === "admin@gmail.com" ? (
                        <Link
                            className="text-xs hover:underline"
                            href="#"
                            onClick={(e) => {
                                e.preventDefault();
                                setOpenFeedback(!openFeedback);
                            }}
                        >
                            Edit
                        </Link>
                    ) : null}
                </div>
                <div className="flex flex-row gap-2 items-center">
                    <Image
                        className="hidden sm:block"
                        src={emailIcon}
                        alt="Email Icon"
                        width={40}
                        height={40}
                    />
                    <p className="text-xs md:text-sm">{contactDetails.email}</p>
                </div>
                <div className="flex flex-row gap-2 items-center">
                    <Image
                        className="hidden sm:block"
                        src={phoneIcon}
                        alt="Phone Icon"
                        width={40}
                        height={40}
                    />
                    <p className="text-xs md:text-sm">
                        {contactDetails.phoneNumber}
                    </p>
                </div>
            </div>
            {openFeedback && (
                <UpdateContactInfoModal
                    contactInfo={contactDetails}
                    opened={openFeedback}
                    onClose={() => setOpenFeedback(false)}
                />
            )}
        </div>
    );
}
