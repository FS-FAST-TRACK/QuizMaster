import { Button, Modal, Space, TextInput, Textarea } from "@mantine/core";
import { ContactDetails, SystemInfoDto } from "@/lib/definitions";
import Link from "next/link";
import { DifficultyCardBody } from "../cards/DifficultyCard";
import { useCallback, useEffect, useState } from "react";
import { useForm } from "@mantine/form";
import { UpdateContactDetails, postContactUs } from "@/lib/hooks/contact-us";
import { UpdateSystemInfo } from "@/lib/hooks/system-info";
import { notification } from "@/lib/notifications";

export default function EditSystemInfoModal({
    systemInfo,
    onClose,
    opened,
}: {
    systemInfo?: SystemInfoDto;
    opened: boolean;
    onClose: () => void;
}) {
    const systemDetails = useForm<SystemInfoDto>({
        initialValues: {
            version: `${systemInfo?.version}`,
            description: `${systemInfo?.description}`,
            web_link: `${systemInfo?.web_link}`,
            mobile_link: `${systemInfo?.mobile_link}`,
            ios_link: `${systemInfo?.ios_link}`,
        },
        clearInputErrorOnChange: true,
        validateInputOnBlur: true,
        validate: {
            version: (value) => {
                if (!value) {
                    return "Version must not be empty.";
                }

                const versionRegex = /^(\d+)\.(\d+)\.(\d+)$/;

                if (!versionRegex.test(value)) {
                    return "Invalid version format. Use the format x.y.z (e.g., 1.0.0)";
                }

                return null;
            },
            description: (value) =>
                value.length < 1 ? "System Info must not be empty." : null,
            web_link: (value) =>
                value.length < 1 ? "Website link must not be empty." : null,
            mobile_link: (value) =>
                value.length < 1 ? "Mobile link must not be empty." : null,
            ios_link: (value) =>
                value.length < 1 ? "iOS link must not be empty." : null,
        },
    });

    const handelSubmit = useCallback(async () => {
        UpdateSystemInfo({ systemDetails: systemDetails.values })
            .then((res) => {
                if (res.status === "Success") {
                    notification({ type: "success", title: res.message });
                    onClose();
                } else {
                    notification({ type: "error", title: res.message });
                }
            })
            .catch((res) => {
                notification({ type: "error", title: ":" + res.message });
            });
    }, [systemDetails.values]);

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            title={
                <div className="font-bold text-2xl text-center">
                    Edit System Information
                </div>
            }
            size="lg"
        >
            <form
                className="space-y-8"
                onSubmit={systemDetails.onSubmit(() => {
                    handelSubmit();
                })}
                onReset={() => systemDetails.reset()}
            >
                <TextInput
                    label="Version"
                    required
                    variant="filled"
                    placeholder="Version"
                    {...systemDetails.getInputProps("version")}
                />
                <Textarea
                    label="System Info"
                    required
                    variant="filled"
                    placeholder="System Info"
                    rows={10}
                    {...systemDetails.getInputProps("description")}
                />
                <TextInput
                    label="Web Link"
                    required
                    variant="filled"
                    placeholder="Web Link"
                    {...systemDetails.getInputProps("web_link")}
                />
                <TextInput
                    label="Mobile Link"
                    required
                    variant="filled"
                    placeholder="Mobile Link"
                    {...systemDetails.getInputProps("mobile_link")}
                />
                <TextInput
                    label="iOS Link"
                    required
                    variant="filled"
                    placeholder="iOS Link"
                    {...systemDetails.getInputProps("ios_link")}
                />
                <div className="flex gap-2">
                    <Button variant="filled" color="orange" type="submit">
                        Submit
                    </Button>
                    <Button variant="outline" color="gray" onClick={onClose}>
                        Close
                    </Button>
                </div>
            </form>
        </Modal>
    );
}
