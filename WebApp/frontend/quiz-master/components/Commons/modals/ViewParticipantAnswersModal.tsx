import {
    QUIZMASTER_QCATEGORY_GET_CATEGORY,
    QUIZMASTER_QDIFFICULTY_GET_DIFFICULTY,
    QUIZMASTER_QTYPE_GET_TYPE,
    QUIZMASTER_QUESTION_GET_QUESTION,
} from "@/api/api-routes";
import { QuizSessionReport } from "@/app/reports/components/sessionsReport";
import { formatDateTimeRange } from "@/lib/dateTimeUtils";
import {
    Question,
    QuestionCategory,
    QuestionDetail,
    QuestionDifficulty,
    QuestionType,
} from "@/lib/definitions";
import {
    CheckCircleIcon,
    ChevronDownIcon,
    EyeIcon,
    InformationCircleIcon,
    XCircleIcon,
} from "@heroicons/react/24/outline";
import { Button, Modal, Popover, Tooltip } from "@mantine/core";
import { useEffect, useState } from "react";
import ViewQuestionScreenShotModal from "./ViewQuestionScreenShotModal";
import { participantAnswersToCsvData } from "@/lib/csvDataGenerator";
import { CSVLink } from "react-csv";
import { PDFDownloadLink } from "@react-pdf/renderer";
import { IndividualParticipantAnswersReport } from "@/app/reports/components/pdfReports/IndividualParticipantAnswersReport";

export default function ViewParticipantAnswersModal({
    opened,
    onClose,
    sessionName,
    sessionReport,
    participantName,
    participantScore,
    participantPlace,
}: {
    opened: boolean;
    onClose: () => void;
    sessionName: string;
    sessionReport: QuizSessionReport;
    participantName: string;
    participantScore: number;
    participantPlace: number;
}) {
    const [questionInfos, setQuestionInfo] = useState<Question[]>([]);

    return (
        <Modal
            size={"xl"}
            opened={opened}
            onClose={onClose}
            title={
                <div className="flex justify-between gap-4 pl-4 pt-4">
                    <div>
                        <p className="text-xs text-gray-500 mb-1">
                            Participant
                        </p>
                        <div className="flex items-center gap-2">
                            <p className="text-2xl text-gray-900 font-bold">
                                {participantName}
                            </p>
                            <span className="text-gray-400">•</span>
                            <p className="text-sm text-gray-400">
                                {participantScore} points{" "}
                            </p>
                        </div>
                    </div>
                </div>
            }
        >
            <div className="px-4 pb-4">
                <div className="rounded-md border border-gray-300 p-3 w-fit mt-3">
                    <div>
                        <h3 className="font-bold text-base text-gray-800 mb-2">
                            {sessionName}
                        </h3>
                        <p className="text-xs text-gray-400 mb-4">
                            {formatDateTimeRange(
                                sessionReport.startTime,
                                sessionReport.endTime
                            )}
                        </p>
                    </div>
                    <div className="mt-2">
                        <p className="text-xs text-gray-800 mb-2">
                            Hosted by: {sessionReport.hostName}
                        </p>
                    </div>
                </div>
                <div className="mb-2 mt-8 flex items-center justify-between">
                    <h2 className="text-base text-gray-800 font-medium">
                        Participant Answers:
                    </h2>
                </div>

                <div>
                    {sessionReport.participantAnswerReports
                        .filter(
                            (report) =>
                                report.participantName === participantName
                        )
                        .map((answer, index) => {
                            return (
                                <ParticipantAnswerItem
                                    key={index}
                                    participantAnswer={answer}
                                    onChangeQuestionInfos={(questionInfo) =>
                                        setQuestionInfo((prev) => [
                                            ...prev,
                                            questionInfo,
                                        ])
                                    }
                                    questionInfos={questionInfos}
                                />
                            );
                        })}
                </div>
            </div>
            <div className="flex gap-2 justify-end mb-4 mt-8">
                <Button variant="default" onClick={onClose}>
                    Cancel
                </Button>
                <Popover width={200} position="bottom" withArrow shadow="md">
                    <Popover.Target>
                        <Button
                            variant="filled"
                            color="green"
                            type="submit"
                            className="bg-[#17a14b]"
                            rightSection={
                                <ChevronDownIcon width={16} height={16} />
                            }
                        >
                            Export Answers
                        </Button>
                    </Popover.Target>
                    <Popover.Dropdown style={{ padding: 0 }}>
                        <div className="overflow-clip bg-white h-fit cursor-pointer text-gray-800 py-2 px-4 flex-col">
                            <CSVLink
                                className="py-2 px-4 hover:font-medium"
                                data={participantAnswersToCsvData(
                                    {
                                        sessionName,
                                        participantName,
                                        participantScore,
                                        participantPlace,
                                        sessionReport,
                                    },
                                    questionInfos
                                )}
                                separator=","
                                filename={`[${sessionName}] ${participantName} Quiz Answers.csv`}
                            >
                                Export as CSV
                            </CSVLink>
                            <div className="py-2 px-4 hover:font-medium">
                                <PDFDownloadLink
                                    fileName={`[${participantName}] - ${sessionName} Answer Reports`}
                                    document={
                                        <IndividualParticipantAnswersReport
                                            questionInfos={questionInfos}
                                            participantAnswers={sessionReport.participantAnswerReports.filter(
                                                (report) =>
                                                    report.participantName ===
                                                    participantName
                                            )}
                                            sessionName={sessionName}
                                            sessionDuration={formatDateTimeRange(
                                                sessionReport.startTime,
                                                sessionReport.endTime
                                            )}
                                            participantScore={participantScore}
                                        />
                                    }
                                >
                                    {({ loading }) =>
                                        loading
                                            ? "Downloading document..."
                                            : "Export as PDF"
                                    }
                                </PDFDownloadLink>
                            </div>
                        </div>
                    </Popover.Dropdown>
                </Popover>
            </div>
        </Modal>
    );
}

export interface ParticipantAnswer {
    id: number;
    sessionId: string;
    participantName: string;
    answer: string;
    screenshotLink: string;
    questionId: number;
}

function ParticipantAnswerItem({
    participantAnswer,
    onChangeQuestionInfos,
    questionInfos,
}: {
    participantAnswer: ParticipantAnswer;
    onChangeQuestionInfos: (questionInfo: Question) => any;
    questionInfos: Question[];
}) {
    const [questionInfo, setQuestionInfo] = useState<Question>();
    const [questionDetail, setQuestionDetail] = useState<QuestionDetail[]>([]);
    const [correctAnswer, setCorrectAnswer] = useState<string>("");
    const [isParticipantAnswerCorrect, setIsParticipantAnswerCorrect] =
        useState<boolean>(false);
    const [openModal, setOpenModal] = useState(false);

    const [qCategory, setQCategory] = useState<QuestionCategory>();
    const [qDifficulty, setQDifficulty] = useState<QuestionDifficulty>();
    const [qType, setQType] = useState<QuestionType>();

    useEffect(() => {
        const fetchQuestionInfo = async () => {
            try {
                const response = await fetch(
                    QUIZMASTER_QUESTION_GET_QUESTION +
                        `${participantAnswer.questionId}`
                );
                const data = await response.json();
                setQuestionInfo(data as Question);
                setQuestionDetail(data.details as QuestionDetail[]);
                setCorrectAnswer(
                    data.details.find((d: QuestionDetail) =>
                        d.detailTypes.includes("answer")
                    )?.qDetailDesc
                );
                onChangeQuestionInfos(data);
            } catch (e) {
                console.log("Something went wrong");
            }
        };
        fetchQuestionInfo();
    }, []);

    useEffect(() => {
        if (
            participantAnswer.answer.trim().toLowerCase() ===
            correctAnswer.toLowerCase()
        ) {
            setIsParticipantAnswerCorrect(true);
        } else {
            const hasMultipleAnswers = correctAnswer.split("|").length !== 0;
            if (hasMultipleAnswers) {
                console.log(
                    "Multiple Answers? ",
                    correctAnswer.split("|").length
                );
            }
            if (hasMultipleAnswers) {
                const possibleAnswers = correctAnswer
                    .split("|")
                    .map((a) => a.trim().toLocaleLowerCase());
                if (
                    possibleAnswers.includes(
                        participantAnswer.answer.toLowerCase()
                    )
                ) {
                    setIsParticipantAnswerCorrect(true);
                } else {
                    setIsParticipantAnswerCorrect(false);
                }
            } else {
                setIsParticipantAnswerCorrect(false);
            }
        }
    }, [questionDetail, correctAnswer]);

    useEffect(() => {
        if (correctAnswer) {
            if (correctAnswer.split("|").length !== 0) {
            }
        }
    }, [correctAnswer]);

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
        <div className="border border-gray-300 rounded-lg p-4 mb-2">
            <div className=" mb-8 ">
                <p className="truncate text-gray-800 font-semibold text-ellipsis mb-2">
                    {questionInfo?.qStatement ||
                        loadingSkeleton("h-8", "w-full")}
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

            <div className="w-full flex items-center justify-between">
                <div className="flex gap-4 item">
                    <div className="flex gap-1">
                        <p
                            className={`text-sm flex gap-1 ${
                                isParticipantAnswerCorrect
                                    ? "text-green-600"
                                    : "text-red-500"
                            }`}
                        >
                            Answered:{" "}
                            {participantAnswer.answer ? (
                                <b
                                    className={`text-gray-900 ${
                                        isParticipantAnswerCorrect
                                            ? "text-green-600"
                                            : "text-red-500"
                                    }`}
                                >
                                    {participantAnswer.answer}
                                </b>
                            ) : (
                                <p className="text-red-500 italic">
                                    {"-- No answer --"}
                                </p>
                            )}
                        </p>
                        <span>
                            {isParticipantAnswerCorrect ? (
                                <CheckCircleIcon
                                    height={20}
                                    width={20}
                                    color="var(--primary)"
                                />
                            ) : (
                                participantAnswer.answer && (
                                    <XCircleIcon
                                        height={20}
                                        width={20}
                                        color="var(--error)"
                                    />
                                )
                            )}
                        </span>
                    </div>

                    {!isParticipantAnswerCorrect && (
                        <p className=" text-sm text-green-600">
                            Corrrect Answer:{" "}
                            <b className="text-green-600">
                                {
                                    questionDetail.find((detail) =>
                                        detail.detailTypes.includes("answer")
                                    )?.qDetailDesc
                                }
                            </b>
                        </p>
                    )}
                    {correctAnswer.split("|").length > 1 &&
                        isParticipantAnswerCorrect && (
                            <Tooltip
                                label={`Possible answers: ${correctAnswer
                                    .split("|")
                                    .join(",")}`}
                                multiline
                                w={220}
                                offset={{ mainAxis: 0, crossAxis: 100 }}
                            >
                                <InformationCircleIcon
                                    className="w-6"
                                    width={22}
                                    height={22}
                                />
                            </Tooltip>
                        )}
                </div>
                {participantAnswer.screenshotLink.length !== 0 ? (
                    <p
                        className="text-gray-500 flex items-center gap-2 text-sm cursor-pointer hover:text-yellow-500 hover:scale-105"
                        onClick={() => setOpenModal(true)}
                    >
                        <span>
                            <EyeIcon width={16} height={16} />
                        </span>
                        Answer Screenshot
                    </p>
                ) : (
                    <p className="text-gray-400 italic flex items-center gap-2 text-sm">
                        No screenshot saved
                    </p>
                )}
            </div>
            <ViewQuestionScreenShotModal
                opened={openModal}
                onClose={() => setOpenModal(false)}
                screenshotLink={participantAnswer.screenshotLink}
                participantName={participantAnswer.participantName}
            />
        </div>
    );
}
