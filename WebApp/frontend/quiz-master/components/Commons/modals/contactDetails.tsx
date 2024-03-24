"use client";
import { useEffect, useState } from "react";
import Image from "next/image";
import emailIcon from "/public/email-icon.png";
import phoneIcon from "/public/telephone-icon.png";
import { getServerSession } from "next-auth";
import Link from "next/link";
import UpdateContactInfoModal from "./UpdateContactInfoModal";
import { ContactDetails } from "@/lib/definitions";
import { fetchContactInfo, fetchLoginUser } from "@/lib/quizData";
import { notification } from "@/lib/notifications";

export default function ContactDetails({ email }: { email?: string }) {
    const [contactDetails, setContactDetails] = useState<ContactDetails>();
    const [openEditContactInfo, setOpenEditContactInfo] =
        useState<boolean>(false);
    const [isAdmin, setIsAdmin] = useState<boolean>(false);

    useEffect(() => {
        fetchLoginUser().then((res) => {
            res?.info?.roles.map((role) => {
                if (role === "Administrator") {
                    setIsAdmin(true);
                }
            });
        });
    }, []);

    useEffect(() => {
        fetchContactInfo()
            .then((res) => {
                setContactDetails(res);
            })
            .catch((res) => {
                notification({ type: "error", title: res.message });
            });
    }, [openEditContactInfo]);

    return (
        <div className="mt-[-100px]">
            <div className=" bg-[#4F4F4F] w-fit p-2 opacity-90 text-white gap-2 rounded-lg">
                <div className="flex flex-row gap-3">
                    <p className=" font-thin text-xs items-center">
                        You can also reach us through the following:
                    </p>
                    {isAdmin ? (
                        <Link
                            className="text-xs hover:underline"
                            href="#"
                            onClick={(e) => {
                                e.preventDefault();
                                setOpenEditContactInfo(!openEditContactInfo);
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
                    <p className="text-xs md:text-sm">
                        {contactDetails?.email}
                    </p>
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
                        {contactDetails?.contact}
                    </p>
                </div>
            </div>
            {openEditContactInfo && (
                <UpdateContactInfoModal
                    contactInfo={contactDetails}
                    opened={openEditContactInfo}
                    onClose={() => setOpenEditContactInfo(false)}
                />
            )}
        </div>
    );
}
