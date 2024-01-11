import { Button, Modal, Textarea } from "@mantine/core";
import { Feedback } from "@/lib/definitions";
import { useCallback } from "react";
import { useForm } from "@mantine/form";
import star from "/public/star.svg";
import starFill from "/public/star-fill.svg";
import Image from "next/image";

export default function FeedbackModal({
    onClose,
    opened,
}: {
    opened: boolean;
    onClose: () => void;
}) {
    const rates: number[] = [1, 2, 3, 4, 5];
    const feedbackDetails = useForm<Feedback>({
        initialValues: {
            rate: 0,
            comment: "",
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            rate: (value) => (value === 0 ? "Rate must not zero." : null),
            comment: (value) =>
                value.length < 1 ? "System Info must not be empty." : null,
        },
    });

    const handelSubmit = useCallback(async () => {}, [feedbackDetails.values]);

    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            centered
            title={
                <div className="flex font-bold text-2xl text-center">
                    Give Feedback
                </div>
            }
            size="md"
        >
            <div className="space-y-8">
                <div className="flex flex-row w-full gap-2 items-center justify-center">
                    {rates.map((rate) => {
                        return (
                            <div
                                key={rate}
                                onClick={(e) => {
                                    e.preventDefault();
                                    feedbackDetails.setFieldValue("rate", rate);
                                }}
                            >
                                {rate > feedbackDetails.values.rate ? (
                                    <Image
                                        src={star}
                                        alt="Contact Us"
                                        width={40}
                                        height={40}
                                    />
                                ) : (
                                    <Image
                                        src={starFill}
                                        alt="Contact Us"
                                        width={40}
                                        height={40}
                                    />
                                )}
                            </div>
                        );
                    })}
                </div>

                <div>
                    <p className=" text-sm">
                        Do you have any thoughts to share?
                    </p>
                    <Textarea
                        label=""
                        required
                        variant="filled"
                        placeholder="Comment"
                        rows={10}
                        {...feedbackDetails.getInputProps("comment")}
                    />
                </div>
                <div className="flex w-full gap-2">
                    <Button
                        variant="filled"
                        color="orange"
                        fullWidth
                        onClick={handelSubmit}
                    >
                        Submit
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
