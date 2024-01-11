"use client";

import { useCallback, useState } from "react";
import { ContactUsCreateValues } from "@/lib/definitions";
import { Button, TextInput, Textarea } from "@mantine/core";
import styles from "@/styles/input.module.css";
import Link from "next/link";
import { useForm } from "@mantine/form";
import { postContactUs } from "@/lib/hooks/contact-us";
import FeedbackModal from "../modals/FeedbackModal";

export default function ContactUsForm() {
    const [openFeedbackModal, setOpenFeedbackModal] = useState<boolean>(false);
    const form = useForm<ContactUsCreateValues>({
        initialValues: {
            firstName: "",
            lastName: "",
            email: "",
            phoneNumber: "",
            message: "",
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            firstName: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
            lastName: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
            email: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
            phoneNumber: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
            message: (value) =>
                value.length < 1
                    ? "Question Statement must not be empty."
                    : null,
        },
    });

    const handelSubmit = useCallback(async () => {
        postContactUs({ contactForm: form.values });
    }, [form.values]);

    return (
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

            {openFeedbackModal && (
                <FeedbackModal
                    opened={openFeedbackModal}
                    onClose={() => setOpenFeedbackModal(false)}
                />
            )}
        </form>
    );
}
