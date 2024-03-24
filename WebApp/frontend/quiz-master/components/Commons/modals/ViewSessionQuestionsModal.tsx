import { QuizSessionReport } from "@/app/reports/components/sessionsReport";
import { formatDateTimeRange } from "@/lib/dateTimeUtils";
import {
    QUIZMASTER_QCATEGORY_GET_CATEGORY,
    QUIZMASTER_QDIFFICULTY_GET_DIFFICULTY,
    QUIZMASTER_QTYPE_GET_TYPE,
} from "@/api/api-routes";
import {
    Question,
    QuestionCategory,
    QuestionDifficulty,
    QuestionType,
} from "@/lib/definitions";
import { Button, Modal } from "@mantine/core";
import { useEffect, useState } from "react";

export function ViewSessionQuestionsModal({
    opened,
    onClose,
    sessionName,
    sessionReport,
    questionInfos,
}: {
    opened: boolean;
    onClose: () => void;
    sessionReport: QuizSessionReport;
    sessionName: string;
    questionInfos: Question[];
}) {
    const ids = questionInfos.map(({ id }) => id);
    const removedDuplicates = questionInfos.filter(
        ({ id }, index) => !ids.includes(id, index + 1)
    );

    return (
        <Modal
            opened={opened}
            onClose={onClose}
            size={"lg"}
            title={
                <div className="flex justify-between gap-4 pl-4 pt-4">
                    <div>
                        <h3 className="font-bold text-2xl text-gray-800 mb-2">
                            {sessionName}
                        </h3>
                        <p className="text-xs text-gray-400">
                            {formatDateTimeRange(
                                sessionReport.startTime,
                                sessionReport.endTime
                            )}
                        </p>
                    </div>
                </div>
            }
        >
            <div className="px-4 pb-4">
                <div className="flex justify-between mt-8 mb-4 items-end">
                    <p className="text-base font-semibold ">{`Questions (${removedDuplicates.length})`}</p>
                </div>
                {removedDuplicates.map((q, index) => {
                    return <QuestionItem key={index} questionInfo={q} />;
                })}
                <div className="flex w-full justify-end mt-8">
                    <Button variant="default" onClick={onClose}>
                        Cancel
                    </Button>
                </div>
            </div>
        </Modal>
    );
}

function QuestionItem({ questionInfo }: { questionInfo: Question }) {
    const [qCategory, setQCategory] = useState<QuestionCategory>();
    const [qDifficulty, setQDifficulty] = useState<QuestionDifficulty>();
    const [qType, setQType] = useState<QuestionType>();

    useEffect(() => {
        const fetchQuestionInfo = () => {
            const response = Promise.all([
                fetch(
                    `${QUIZMASTER_QCATEGORY_GET_CATEGORY}${questionInfo?.qCategoryId}`
                ).then((response) => response.json()),
                fetch(
                    `${QUIZMASTER_QTYPE_GET_TYPE}${questionInfo?.qTypeId}`
                ).then((response) => response.json()),
                fetch(
                    `${QUIZMASTER_QDIFFICULTY_GET_DIFFICULTY}${questionInfo?.qDifficultyId}`
                ).then((response) => response.json()),
            ]);

            response.then(([categoryReq, typeReq, difficultyReq]) => {
                setQCategory(categoryReq as QuestionCategory);
                setQType(typeReq as QuestionType);
                setQDifficulty(difficultyReq as QuestionDifficulty);
            });
        };
        if (questionInfo) {
            fetchQuestionInfo();
        }
    }, [questionInfo]);

    const loadingSkeleton = (
        height: "h-4" | "h-8" | "h-16" = "h-4",
        width: "w-full" | "w-16" | "w-32" | "w-64" = "w-16"
    ) => (
        <div
            className={`bg-gray-300 ${height} rounded-sm ${width} animate-pulse w-`}
        ></div>
    );

    return (
        <div className="border border-gray-200 rounded-md p-3 mb-2">
            <div className=" mb-4 ">
                <p className="text-sm font-medium mb-2">
                    {questionInfo.qStatement}
                </p>
                <div className="flex gap-1">
                    <p className="text-gray-400 text-xs">
                        {qType?.qTypeDesc || loadingSkeleton("h-4")}
                    </p>
                    <span className="text-gray-400 text-xs font-light">•</span>
                    <p className="text-gray-400 text-xs">
                        {qDifficulty?.qDifficultyDesc || loadingSkeleton("h-4")}{" "}
                    </p>
                    <span className="text-gray-400 text-xs font-light">•</span>
                    <p className="text-gray-400 text-xs">
                        {qCategory?.qCategoryDesc || loadingSkeleton("h-4")}
                    </p>
                </div>
            </div>
            <div className="flex gap-2">
                <p className="text-sm">Answer: </p>
                <p className="text-sm font-medium text-green-500">
                    {questionInfo.details.find((d) =>
                        d.detailTypes.includes("answer")
                    )?.qDetailDesc || ""}
                </p>
            </div>
        </div>
    );
}
