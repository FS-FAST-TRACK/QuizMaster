import { Button, Modal, Space, TextInput } from "@mantine/core";
import { ContactDetails } from "@/lib/definitions";
import Link from "next/link";
import { DifficultyCardBody } from "../cards/DifficultyCard";
import { useCallback, useEffect, useState } from "react";
import { useForm } from "@mantine/form";
import { UpdateContactDetails } from "@/lib/hooks/contact-us";
import { notification } from "@/lib/notifications";

export default function UpdateContactInfoModal({
    contactInfo,
    onClose,
    opened,
}: {
    contactInfo?: ContactDetails;
    opened: boolean;
    onClose: () => void;
}) {
    const contactDetails = useForm<ContactDetails>({
        initialValues: {
            email: `${contactInfo?.email}`,
            contact: `${contactInfo?.contact}`,
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            email: (value) =>
                value.length < 1 ? "Email must not be empty." : null,
            contact: (value) =>
                value.length < 1 ? "Phone number must not be empty." : null,
        },
    });

    const handelSubmit = useCallback(async () => {
        UpdateContactDetails({ contactForm: contactDetails.values })
            .then((res) => {
                if (res.status === "Success") {
                    notification({ type: "success", title: res.message });
                    onClose();
                } else {
                    notification({ type: "error", title: res.message });
                }
            })
            .catch(() => {
                notification({ type: "error", title: "Something went wrong" });
            });
    }, [contactDetails.values]);

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            title={
                <div className="font-bold text-2xl text-center">
                    Edit Contact Us Information
                </div>
            }
            size="md"
        >
            <div className="space-y-8">
                <TextInput
                    label="Email"
                    required
                    variant="filled"
                    placeholder="Email"
                    {...contactDetails.getInputProps("email")}
                />
                <TextInput
                    label="Phone Number"
                    required
                    variant="filled"
                    placeholder="Phone Number"
                    {...contactDetails.getInputProps("contact")}
                />
                <div className="flex gap-2">
                    <Button
                        variant="filled"
                        color="orange"
                        onClick={handelSubmit}
                    >
                        Submit
                    </Button>
                    <Button variant="outline" color="gray" onClick={onClose}>
                        Close
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
