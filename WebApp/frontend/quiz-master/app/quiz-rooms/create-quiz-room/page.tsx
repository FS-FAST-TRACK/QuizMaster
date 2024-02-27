"use client";

import {
    Anchor,
    Breadcrumbs,
    Button,
    Checkbox,
    InputLabel,
    LoadingOverlay,
    Table,
    TextInput,
    Tooltip,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { useCallback, useState } from "react";
import styles from "@/styles/input.module.css";
import { CreateQuizRoom, RoomOptionTypes } from "@/lib/definitions/quizRoom";
import { validatorFactory } from "@/lib/validation/creators";
import { isRequired } from "@/lib/validation/regex";
import { validate } from "@/lib/validation/validate";
import { InformationCircleIcon } from "@heroicons/react/24/outline";
import { fetchSets } from "@/lib/quizData";
import { useQuery } from "@tanstack/react-query";
import { postQuizRoom } from "@/lib/hooks/quizRoom";
import toast from "react-hot-toast";
import { useRouter } from "next/navigation";

const items = [
    { label: "All", href: "/quiz-rooms" },
    { label: "Create a Quiz Room", href: "#" },
    { label: "", href: "#" },
].map((item, index) => (
    <Anchor href={item.href} key={index}>
        <p className="text-black">{item.label}</p>
    </Anchor>
));

const maxChar = validatorFactory(100, "max");
const minChar = validatorFactory(3, "min");

export default function Page() {
    const router = useRouter();
    const [visible, { close, open }] = useDisclosure(false);
    const [selectedQuestionSets, setSelectedQuestionSets] = useState<number[]>(
        []
    );

    const form = useForm<CreateQuizRoom>({
        initialValues: {
            roomName: "",
            questionSets: [],
            roomOptions: [
                "mode:normal",
                "displaytop10only:false",
                "allowjoinonquizstarted:false",
                "allowreconnect:false",
                "showLeaderboardEachRound:false",
            ],
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            roomName: (value, values) => {
                const validators = [isRequired, minChar, maxChar];
                return validate(value, validators);
            },
        },
    });

    // Creates new quiz room
    const handelSubmit = useCallback(async () => {
        open();

        const quizRoom: CreateQuizRoom = {
            roomName: form.values.roomName,
            questionSets: selectedQuestionSets,
            roomOptions: form.values.roomOptions as RoomOptionTypes,
        };
        // Call the api to create a new quiz room
        const response = await postQuizRoom(quizRoom, () => {
            setTimeout(() => {
                router.push("/quiz-rooms");
            }, 5000);
        });
        toast(response.message);

        close();
    }, [form.values]);

    // toggles the mode for quizRoom
    const toggleMode = useCallback(() => {
        var options = form.values.roomOptions;
        const modeIndex = options.findIndex(
            (o) => o === "mode:normal" || o === "mode:elimination"
        );
        if (modeIndex !== -1) {
            const deleted = options.splice(modeIndex, 1);
            if (deleted[0] === "mode:normal") {
                options = [...options, "mode:elimination"];
            } else {
                options = [...options, "mode:normal"];
            }
        } else {
            options = [...options, "mode:normal"];
        }
        form.setFieldValue("roomOptions", options);
    }, [form.values]);

    const toogleOptions = useCallback(
        (
            option:
                | "showLeaderboardEachRound"
                | "displaytop10only"
                | "allowreconnect"
                | "allowjoinonquizstarted"
        ) => {
            var options = form.values.roomOptions;
            var optionIndex = options.findIndex((o) => o.startsWith(option));
            if (optionIndex !== -1) {
                const deleted = options.splice(optionIndex, 1);
                if (deleted[0].endsWith("false")) {
                    options = [...options, `${option}:true`];
                } else {
                    options = [...options, `${option}:false`];
                }
            } else {
                options = [...options, `${option}:true`];
            }
            form.setFieldValue("roomOptions", options);
        },
        [form.values]
    );

    const {
        data: questionSets,
        isLoading,
        error,
    } = useQuery({
        queryKey: ["questionSets"],
        queryFn: fetchSets,
    });

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row justify-between text-2xl font-bold">
                <h3>Create New Quiz Room</h3>
            </div>
            <form
                className="flex flex-col gap-8 relative"
                onSubmit={form.onSubmit((values) => {
                    handelSubmit();
                })}
                onReset={() => form.reset()}
            >
                <LoadingOverlay
                    visible={visible}
                    zIndex={1000}
                    overlayProps={{ radius: "sm", blur: 2 }}
                />
                <TextInput
                    label="Room Name"
                    variant="filled"
                    withAsterisk
                    classNames={styles}
                    placeholder="Room name"
                    {...form.getInputProps("roomName")}
                />

                {isLoading ? (
                    <InputLabel>Question Sets</InputLabel>
                ) : questionSets === undefined ? null : (
                    <div>
                        <InputLabel>Question Sets</InputLabel>
                        <Table striped>
                            <Table.Thead>
                                <Table.Tr>
                                    <Table.Th>
                                        <Checkbox
                                            color="green"
                                            aria-label="Select row"
                                            checked={
                                                selectedQuestionSets.length ===
                                                questionSets?.length
                                            }
                                            onChange={(event) =>
                                                setSelectedQuestionSets(
                                                    event.currentTarget.checked
                                                        ? questionSets.map(
                                                              (questionSet) =>
                                                                  questionSet.id
                                                          )
                                                        : []
                                                )
                                            }
                                        />
                                    </Table.Th>
                                    <Table.Th>Question Set</Table.Th>
                                    <Table.Th>Created on</Table.Th>
                                    <Table.Th>Updated on</Table.Th>
                                    <Table.Th>No. of questions</Table.Th>
                                </Table.Tr>
                            </Table.Thead>
                            <Table.Tbody>
                                {questionSets.map((questionSet) => (
                                    <Table.Tr key={questionSet.id}>
                                        <Table.Td>
                                            <Checkbox
                                                color="green"
                                                aria-label="Select row"
                                                checked={selectedQuestionSets.includes(
                                                    questionSet.id
                                                )}
                                                onChange={(event) =>
                                                    setSelectedQuestionSets(
                                                        event.currentTarget
                                                            .checked
                                                            ? [
                                                                  ...selectedQuestionSets,
                                                                  questionSet.id,
                                                              ]
                                                            : selectedQuestionSets.filter(
                                                                  (id) =>
                                                                      id !==
                                                                      questionSet.id
                                                              )
                                                    )
                                                }
                                            />
                                        </Table.Td>
                                        <Table.Td>
                                            {questionSet.qSetName}
                                        </Table.Td>
                                        <Table.Td>
                                            {new Date(
                                                questionSet.dateCreated
                                            ).toLocaleDateString()}
                                        </Table.Td>
                                        <Table.Td>
                                            {new Date(
                                                questionSet.dateUpdated
                                            ).toLocaleDateString()}
                                        </Table.Td>
                                        <Table.Td>
                                            {questionSet.numberOfQuestions}
                                        </Table.Td>
                                    </Table.Tr>
                                ))}
                            </Table.Tbody>
                        </Table>
                    </div>
                )}

                <div>
                    <InputLabel>Mode</InputLabel>
                    <div className="flex gap-10">
                        <Checkbox
                            label="Normal"
                            color="green"
                            checked={form.values.roomOptions.includes(
                                "mode:normal"
                            )}
                            onChange={(e) => {
                                toggleMode();
                            }}
                        />
                        <div className="flex">
                            <Checkbox
                                label="Elimination"
                                color="green"
                                checked={form.values.roomOptions.includes(
                                    "mode:elimination"
                                )}
                                onChange={(e) => {
                                    toggleMode();
                                }}
                            />
                            <Tooltip
                                label="Only top 50% of the participants willproceed to the next round"
                                multiline
                                w={220}
                                offset={{ mainAxis: 0, crossAxis: 100 }}
                            >
                                <InformationCircleIcon className="w-6" />
                            </Tooltip>
                        </div>
                    </div>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-5 max-w-2xl">
                    <Checkbox
                        label="Allow reconnect"
                        color="green"
                        checked={form.values.roomOptions.includes(
                            "allowreconnect:true"
                        )}
                        onChange={() => {
                            toogleOptions("allowreconnect");
                        }}
                    />
                    <Checkbox
                        label="Show leaderboard each round"
                        color="green"
                        checked={form.values.roomOptions.includes(
                            "showLeaderboardEachRound:true"
                        )}
                        onChange={() => {
                            toogleOptions("showLeaderboardEachRound");
                        }}
                    />
                    <Checkbox
                        id="allow-join"
                        label="Allow join on game started"
                        color="green"
                        onChange={() => {
                            toogleOptions("allowjoinonquizstarted");
                        }}
                        checked={form.values.roomOptions.includes(
                            "allowjoinonquizstarted:true"
                        )}
                    />
                    <Checkbox
                        id="top-10-only"
                        label="Display top 10 participants only"
                        color="green"
                        onChange={() => {
                            toogleOptions("displaytop10only");
                        }}
                        checked={form.values.roomOptions.includes(
                            "displaytop10only:true"
                        )}
                    />
                </div>

                <div className="flex justify-end">
                    <Button
                        variant="transparent"
                        color="gray"
                        type="reset"
                        onClick={() => {
                            router.push("/quiz-rooms");
                        }}
                    >
                        Cancel
                    </Button>
                    <Button variant="filled" color="green" type="submit">
                        Create Quiz Room
                    </Button>
                </div>
            </form>
        </div>
    );
}
