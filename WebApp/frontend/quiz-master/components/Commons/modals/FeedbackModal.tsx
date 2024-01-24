import { Button, Input, Modal, Textarea } from "@mantine/core";
import { Feedback } from "@/lib/definitions";
import { useCallback, useEffect, useState } from "react";
import { useForm } from "@mantine/form";
import star from "/public/star.svg";
import starFill from "/public/star-fill.svg";
import Image from "next/image";
import { postReachOut } from "@/lib/hooks/contact-us";
import { notification } from "@/lib/notifications";

export default function FeedbackModal({
    onClose,
    opened,
}: {
    opened: boolean;
    onClose: () => void;
}) {
    const [onSubmit, setOnSubmit] = useState<boolean>(false);
    const rates: number[] = [1, 2, 3, 4, 5];
    const feedbackDetails = useForm<Feedback>({
        initialValues: {
            starRating: 0,
            comment: "",
        },
        clearInputErrorOnChange: true,
        validateInputOnBlur: true,
        validate: {
            starRating: (value) => (value === 0 ? "Minimum star is 1." : null),
            comment: (value) =>
                value.length < 1 ? "Comment must not be empty." : null,
        },
    });

    useEffect(() => {
        setOnSubmit(false);
    }, [feedbackDetails.values.starRating]);

    const handelSubmit = useCallback(async () => {
        postReachOut({ feedbackForm: feedbackDetails.values })
            .then((res) => {
                console.log(res);
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
    }, [feedbackDetails.values]);

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
            <form
                className="space-y-8"
                onSubmit={feedbackDetails.onSubmit(() => {
                    handelSubmit();
                })}
                onReset={() => feedbackDetails.reset()}
            >
                <div className="flex flex-row w-full gap-2 items-center justify-center">
                    {rates.map((rate) => {
                        return (
                            <div
                                key={rate}
                                onClick={(e) => {
                                    e.preventDefault();
                                    feedbackDetails.setFieldValue(
                                        "starRating",
                                        rate
                                    );
                                }}
                            >
                                {rate > feedbackDetails.values.starRating ? (
                                    <Image
                                        src={star}
                                        alt="Star"
                                        width={40}
                                        height={40}
                                    />
                                ) : (
                                    <Image
                                        src={starFill}
                                        alt="Star"
                                        width={40}
                                        height={40}
                                    />
                                )}
                            </div>
                        );
                    })}
                </div>

                {!feedbackDetails.isValid("starRating") && onSubmit ? (
                    <Input.Label style={{ color: "red", opacity: 0.7 }}>
                        Note: Minimum star is 1.
                    </Input.Label>
                ) : null}

                <div>
                    <Textarea
                        label="Do you have any thoughts to share?"
                        variant="filled"
                        placeholder="Comment"
                        required
                        rows={10}
                        {...feedbackDetails.getInputProps("comment")}
                    />
                </div>
                <div
                    className="flex w-full gap-2"
                    onClick={() => {
                        setOnSubmit(true);
                    }}
                >
                    <Button
                        variant="filled"
                        color="orange"
                        fullWidth
                        type="submit"
                    >
                        Submit
                    </Button>
                </div>
            </form>
        </Modal>
    );
}
