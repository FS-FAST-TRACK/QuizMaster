import { Button, Modal, Space, TextInput, Textarea } from "@mantine/core";
import { ContactDetails, SystemInfoDto } from "@/lib/definitions";
import Link from "next/link";
import { DifficultyCardBody } from "../cards/DifficultyCard";
import { useCallback, useEffect, useState } from "react";
import { useForm } from "@mantine/form";
import { UpdateContactDetails, postContactUs } from "@/lib/hooks/contact-us";
import { UpdateSystemInfo } from "@/lib/hooks/system-info";

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
            systemInfo: `${systemInfo?.systemInfo}`,
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            version: (value) =>
                value.length < 1 ? "Version must not be empty." : null,
            systemInfo: (value) =>
                value.length < 1 ? "System Info must not be empty." : null,
        },
    });

    const handelSubmit = useCallback(async () => {
        UpdateSystemInfo({ systemDetails: systemDetails.values });
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
            size="md"
        >
            <div className="space-y-8">
                <TextInput
                    label="Version"
                    required
                    variant="filled"
                    placeholder="Email"
                    {...systemDetails.getInputProps("version")}
                />
                <Textarea
                    label="System Info"
                    required
                    variant="filled"
                    placeholder="Phone Number"
                    rows={10}
                    {...systemDetails.getInputProps("systemInfo")}
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
