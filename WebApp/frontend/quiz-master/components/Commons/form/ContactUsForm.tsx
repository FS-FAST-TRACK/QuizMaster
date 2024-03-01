"use client";

import { useCallback, useEffect, useState } from "react";
import { ContactUsCreateValues, UserInfo } from "@/lib/definitions";
import { Button, TextInput, Textarea } from "@mantine/core";
import styles from "@/styles/input.module.css";
import Link from "next/link";
import { useForm } from "@mantine/form";
import { postContactUs } from "@/lib/hooks/contact-us";
import FeedbackModal from "../modals/FeedbackModal";
import { fetchLoginUser } from "@/lib/quizData";
import { notification } from "@/lib/notifications";

export default function ContactUsForm() {
    const [openFeedbackModal, setOpenFeedbackModal] = useState<boolean>(false);
    const form = useForm<ContactUsCreateValues>({
        initialValues: {
            userId: 0,
            firstName: "",
            lastName: "",
            email: "",
            phoneNumber: "",
            message: "",
        },
        clearInputErrorOnChange: true,
        validateInputOnBlur: true,
        validate: {
            firstName: (value) =>
                value.length < 1 ? "First Name must not be empty." : null,
            lastName: (value) =>
                value.length < 1 ? "Last Name must not be empty." : null,
            email: (value) => {
                if (!value) {
                    return "Email must not be empty.";
                }
                return /^\S+@\S+$/.test(value) ? null : "Invalid email.";
            },
            phoneNumber: (value) => {
                if (!value) {
                    return "Phone Number must not be empty.";
                }

                const phoneNumberRegex = /^09\d{9}$/;

                return phoneNumberRegex.test(value)
                    ? null
                    : "Invalid phone number (Must start with 09 and a total of 11 digits).";
            },
            message: (value) =>
                value.length < 1 ? "Message must not be empty." : null,
        },
    });

    useEffect(() => {
        fetchLoginUser().then((res) => {
            if (res) {
                setForm(res);
            }
        });
    }, []);

    const setForm = (user: UserInfo) => {
        form.setFieldValue(
            "userId",
            user.info.userData.id ? user.info.userData.id : 0
        );
        form.setFieldValue(
            "firstName",
            user.info.userData.firstName ? user.info.userData.firstName : ""
        );
        form.setFieldValue(
            "lastName",
            user.info.userData.lastName ? user.info.userData.lastName : ""
        );
        form.setFieldValue(
            "email",
            user.info.userData.email ? user.info.userData.email : ""
        );
        form.setFieldValue(
            "phoneNumber",
            user.info.userData.phoneNumber ? user.info.userData.phoneNumber : ""
        );
    };

    const handelSubmit = useCallback(async () => {
        postContactUs({ contactForm: form.values })
            .then((res) => {
                if (res.status === "Success") {
                    form.reset();
                    notification({
                        type: "success",
                        title: res.message,
                    });
                } else {
                    notification({
                        type: "error",
                        title: res.message,
                    });
                }
            })
            .catch(() => {
                notification({ type: "error", title: "Something went wrong" });
            });
    }, [form.values]);

    return (
        <>
            <form
                className="flex flex-col gap-4 w-full flex-1"
                onSubmit={form.onSubmit(() => {
                    handelSubmit();
                })}
                onReset={() => form.reset()}
            >
                <div className="flex flex-row gap-5">
                    <div className="flex-1">
                        <TextInput
                            label="First Name"
                            variant="filled"
                            withAsterisk
                            classNames={styles}
                            placeholder="First Name"
                            {...form.getInputProps("firstName")}
                        />
                    </div>
                    <div className="flex-1">
                        <TextInput
                            label="Last Name"
                            variant="filled"
                            withAsterisk
                            classNames={styles}
                            placeholder="Last Name"
                            {...form.getInputProps("lastName")}
                        />
                    </div>
                </div>
                <div>
                    <TextInput
                        label="Email"
                        variant="filled"
                        withAsterisk
                        classNames={styles}
                        placeholder="Email"
                        autoComplete="email"
                        {...form.getInputProps("email")}
                    />
                </div>
                <div>
                    <TextInput
                        label="Phone Number"
                        variant="filled"
                        withAsterisk
                        classNames={styles}
                        placeholder="Phone Number"
                        {...form.getInputProps("phoneNumber")}
                    />
                </div>
                <div>
                    <Textarea
                        label="Message"
                        placeholder="Message"
                        variant="filled"
                        autosize
                        minRows={7}
                        {...form.getInputProps("message")}
                    />
                </div>
                <div>
                    <Button variant="filled" color="orange" type="submit">
                        Submit
                    </Button>
                </div>
                <div className="flex flex-row gap-2 text-sm">
                    <p>
                        If you’d like to give us your feedback,&nbsp;
                        <Link
                            href="#"
                            className=" text-orange-500 hover:underline"
                            onClick={(e) => {
                                e.preventDefault();
                                setOpenFeedbackModal(true);
                            }}
                        >
                            click here.
                        </Link>
                    </p>
                </div>
            </form>
            {openFeedbackModal && (
                <FeedbackModal
                    opened={openFeedbackModal}
                    onClose={() => setOpenFeedbackModal(false)}
                />
            )}
        </>
    );
}
