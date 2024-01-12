"use client";

import { QuestionDetail } from "@/lib/definitions";
import { useForm } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";
import MultipleChoicePlusAudioQuestionDetail from "./MultipleChoicePlusAudio";
import TrueOrFalseQuestionDetail from "./TrueOrFalse";
import TypeAnswerQuestionDetails from "./TypeAnwer";
import SliderQuestionDetails from "./Slider";
import PuzzleQuestionDetails from "./Puzzle";
import {
    MultipleChoiceData,
    MultipleChoicePlusAudioData,
    PuzzleData,
    SliderData,
} from "@/lib/questionTypeData";
import { useCallback, useEffect, useState } from "react";
import { Button } from "@mantine/core";
import {
    deleteQuestionDetail,
    patchQuestionDetail,
    postQuestionDetail,
    fetchQuestionDetails,
} from "@/lib/hooks/questionDetails";
import { isRequired } from "@/lib/validation/regex";
import { notification } from "@/lib/notifications";
import { getTransformedData } from "./utils";
import { useErrorRedirection } from "@/utils/errorRedirection";

export default function QuestionDetails({
    questionId,
    questionTypeId,
    details,
}: {
    questionId: number;
    questionTypeId: string;
    details?: QuestionDetail[];
}) {
    const [oldDetails, setOldDetails] = useState<QuestionDetail[]>([]);
    const { redirectToError } = useErrorRedirection();

    const form = useForm<{ details: QuestionDetail[] }>({
        initialValues: {
            details: details || [],
        },
        clearInputErrorOnChange: true,
        validateInputOnChange: true,
        validate: {
            details: {
                qDetailDesc: (value, values, path) => {
                    if (!value) {
                        return "Provide content.";
                    }

                    if (
                        parseInt(questionTypeId) === MultipleChoiceData.id ||
                        parseInt(questionTypeId) ===
                            MultipleChoicePlusAudioData.id
                    ) {
                        return values.details.findIndex((op, i) => {
                            return (
                                op.qDetailDesc === value &&
                                path !== `details.${i}.qDetailDesc` &&
                                op.detailTypes.includes("option")
                            );
                        }) >= 0
                            ? "Duplicated Choice."
                            : null;
                    }

                    if (parseInt(questionTypeId) === SliderData.id) {
                        var min = values.details.find((qDetail) =>
                            qDetail.detailTypes.includes("minimum")
                        );
                        var max = values.details.find((qDetail) =>
                            qDetail.detailTypes.includes("maximum")
                        );
                        var interval = values.details.find((qDetail) =>
                            qDetail.detailTypes.includes("interval")
                        );
                        var answer = values.details.find((qDetail) =>
                            qDetail.detailTypes.includes("answer")
                        );
                        // check error for minimum
                        if (
                            values.details.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("minimum") &&
                                    path === `details.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide minimmum";
                            return max?.qDetailDesc &&
                                parseInt(value) > parseInt(max?.qDetailDesc)
                                ? "Minimum must not be larger than maximum"
                                : answer?.qDetailDesc &&
                                    parseInt(answer.qDetailDesc) <
                                        parseInt(value)
                                  ? "Minimum must not be larger than the answer"
                                  : null;
                        }

                        // check error for maximum
                        if (
                            values.details.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("maximum") &&
                                    path === `details.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide maximum";
                            return min?.qDetailDesc &&
                                parseInt(value) < parseInt(min?.qDetailDesc)
                                ? "Maximum must not be smaller than minimim"
                                : answer?.qDetailDesc &&
                                    parseInt(answer.qDetailDesc) >
                                        parseInt(value)
                                  ? "Maximum must not be smaller than the answer"
                                  : null;
                        }

                        // check error for interval
                        if (
                            values.details.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("interval") &&
                                    path === `details.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide interval";
                            return answer?.qDetailDesc &&
                                min?.qDetailDesc &&
                                (parseInt(answer.qDetailDesc) -
                                    parseInt(min.qDetailDesc)) %
                                    parseInt(value) !==
                                    0
                                ? "Answer can't be hit with the given interval."
                                : null;
                        }

                        // check error for answer
                        if (
                            values.details.find(
                                (qDetail, i) =>
                                    qDetail.detailTypes.includes("answer") &&
                                    path === `details.${i}.qDetailDesc`
                            )
                        ) {
                            if (!value) return "Provide answer";
                            return min?.qDetailDesc && // check if answer is smaller than minimum
                                parseInt(min.qDetailDesc) > parseInt(value)
                                ? "Answer can't be smaller than the minimum."
                                : max?.qDetailDesc &&
                                    parseInt(max.qDetailDesc) < parseInt(value) // check if answer is larger than the maximum
                                  ? "Answer can't be larger than the maximum."
                                  : interval?.qDetailDesc &&
                                      min?.qDetailDesc &&
                                      (parseInt(value) -
                                          parseInt(min.qDetailDesc)) %
                                          parseInt(interval.qDetailDesc) !==
                                          0 // check if answer can be hit with given interval
                                    ? "Answer can't be hit with the givent interval"
                                    : null;
                        }
                    }

                    if (
                        !value &&
                        (parseInt(questionTypeId) === MultipleChoiceData.id ||
                            parseInt(questionTypeId) ===
                                MultipleChoicePlusAudioData.id)
                    ) {
                        return "Provide option";
                    }

                    if (parseInt(questionTypeId) === PuzzleData.id) {
                        return values.details.findIndex((op, i) => {
                            return (
                                op.qDetailDesc === value &&
                                path !== `details.${i}.qDetailDesc` &&
                                op.detailTypes.includes("option")
                            );
                        }) >= 0
                            ? "Duplicated Choice"
                            : null;
                    }
                    return null;
                },
                detailTypes: (value, values) => {
                    return null;
                },
            },
        },
    });

    useEffect(() => {
        if (oldDetails) {
            form.setInitialValues({ details: oldDetails });
            form.setValues({ details: oldDetails });
        }
    }, [oldDetails]);

    const populateOldDetails = useCallback(async () => {
        try {
            const res = await fetchQuestionDetails({ questionId: questionId });
            // filter out the question details
            var filteredDetails: QuestionDetail[] = res.data;

            // special block of code to handle options positioning
            if (questionTypeId === PuzzleData.id.toString()) {
                try {
                    // get the answer detail for puzzle questions
                    var answerDetail = filteredDetails.find((q) =>
                        q.detailTypes.includes("answer")
                    );
                    var answerDescription = answerDetail!.qDetailDesc;

                    const answer: number[] = JSON.parse(
                        answerDescription[0] !== "["
                            ? `[${answerDescription}]`
                            : answerDescription
                    );
                    var filteredDetails = answer.map(
                        (id) => filteredDetails.find((d) => d.id === id)!
                    );
                } catch (error) {
                    notification({
                        type: "error",
                        title: "This puzzle queston doesn't have an answer.",
                    });
                    throw new Error("No answer found");
                }
            }

            setOldDetails(filteredDetails);
        } catch (error) {
            redirectToError();
        }
    }, [questionId]);

    useEffect(() => {
        populateOldDetails();
    }, [questionId, populateOldDetails]);

    const updateDetails = useCallback(async () => {
        const {
            questionDetailsCreateRequest,
            questionDetailsPatchRequest,
            questionDetailsDeleteIds,
        } = getTransformedData(form, oldDetails);

        const promise = questionDetailsPatchRequest.map((qRequest) => {
            try {
                const res = patchQuestionDetail({
                    questionId: questionId,
                    id: qRequest.id,
                    patchRequest: qRequest.patchRequest,
                });
                return res;
            } catch (error) {
                throw new Error("Failed to patch question details");
            }
        });
        const postRequest = questionDetailsCreateRequest.map((qRequest) => {
            try {
                const res = postQuestionDetail({
                    questionId: questionId,
                    questionDetail: {
                        qDetailDesc: qRequest.qDetailDesc,
                        detailTypes: qRequest.detailTypes,
                    },
                });
                return res;
            } catch (error) {
                throw new Error("Failed to post question details");
            }
        });
        const deleteRequests = questionDetailsDeleteIds.map((detailId) => {
            try {
                const res = deleteQuestionDetail({
                    questionId: questionId,
                    id: detailId,
                });
                return res;
            } catch (error) {
                throw new Error("Failed to delete question details");
            }
        });

        const post = Promise.all(postRequest);

        const update = Promise.all(promise);

        const deletePromice = Promise.all(deleteRequests);

        await Promise.all([post, update, deletePromice])
            .then(([postRes, updateRes, deleteRes]) => {
                notification({
                    type: "success",
                    title: "Successfuly updated question details.",
                });

                return { postRes, updateRes, deleteRes };
            })
            .catch(() => {
                notification({ type: "error", title: "Something went wrong" });
                redirectToError();
            });

        // special code added to handle puzzle question positioning.
        if (questionTypeId === PuzzleData.id.toString()) {
            try {
                const res = await fetchQuestionDetails({
                    questionId: questionId,
                });

                // get the question details
                const details: QuestionDetail[] = res.data;

                // get the answer Details
                const answerDetail = details.find((qd) =>
                    qd.detailTypes.includes("answer")
                );

                var newAnswer: number[] = [];
                form.values.details.forEach((d) => {
                    if (d.id) {
                        newAnswer = [...newAnswer, d.id];
                    } else {
                        const detail = details.find(
                            (d2) =>
                                d2.qDetailDesc.trim().toLocaleLowerCase() ===
                                    d.qDetailDesc.trim().toLocaleLowerCase() &&
                                d2.detailTypes.includes("option")
                        );
                        if (detail) {
                            newAnswer = [...newAnswer, detail.id];
                        }
                    }
                });

                const answerRes = await patchQuestionDetail({
                    questionId: questionId,
                    id: answerDetail!.id,
                    patchRequest: [
                        {
                            op: "replace",
                            value: `[${newAnswer.toString()}]`,
                            path: "qDetailDesc",
                        },
                    ],
                });
            } catch (error) {
                notification({
                    type: "error",
                    title: "Something went wrong when updating the puzzle questions answer",
                });
            }
        }
        form.resetDirty();
        populateOldDetails();
    }, [form]);

    const component =
        questionTypeId === "1" ? (
            <MultipleChoiceQuestionDetail form={form} />
        ) : questionTypeId === "2" ? (
            <MultipleChoicePlusAudioQuestionDetail form={form} />
        ) : questionTypeId === "3" ? (
            <TrueOrFalseQuestionDetail form={form} />
        ) : questionTypeId === "4" ? (
            <TypeAnswerQuestionDetails form={form} />
        ) : questionTypeId === "5" ? (
            <SliderQuestionDetails form={form} />
        ) : questionTypeId === "6" ? (
            <PuzzleQuestionDetails form={form} />
        ) : null;
    return (
        <form
            onReset={() => {
                form.reset();
            }}
            onSubmit={(e) => {
                e.preventDefault();
                form.validate();
                updateDetails();
            }}
        >
            {component}
            <div className="mt-5 flex justify-end gap-5">
                <Button
                    variant="transparent"
                    color="gray"
                    disabled={!form.isDirty()}
                    type="reset"
                >
                    Cancel
                </Button>
                <Button
                    variant="filled"
                    color="green"
                    disabled={!form.isDirty() || !form.isValid()}
                    type="submit"
                >
                    Update
                </Button>
            </div>
        </form>
    );
}
