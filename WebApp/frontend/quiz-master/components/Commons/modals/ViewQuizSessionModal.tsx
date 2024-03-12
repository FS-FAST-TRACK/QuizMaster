import { QUIZMASTER_QUESTION_GET_QUESTION } from "@/api/api-routes";
import LeaderBoard from "@/app/reports/components/leaderboard";
import { QuizSessionReportPDF } from "@/app/reports/components/pdfReports/QuizSessionReportPDF";
import { QuizSessionReport } from "@/app/reports/components/sessionsReport";
import { answersAllParticipantsToCsvData } from "@/lib/csvDataGenerator";
import { formatDateTimeRange } from "@/lib/dateTimeUtils";
import { Question } from "@/lib/definitions";
import { UserIcon, ChevronDownIcon } from "@heroicons/react/24/outline";
import { Button, Modal, ModalHeader, Popover } from "@mantine/core";
import { PDFDownloadLink } from "@react-pdf/renderer";
import { useEffect, useState } from "react";
import { CSVLink } from "react-csv";
import { ViewSessionQuestionsModal } from "./ViewSessionQuestionsModal";

export default function ViewQuizSessionModal({
    opened,
    onClose,
    sessionReport,
    sessionName,
}: {
    opened: boolean;
    onClose: () => void;
    sessionReport: QuizSessionReport;
    sessionName: string;
}) {
    const questionIds = [
        ...new Set(
            sessionReport.participantAnswerReports.map(
                (participantAnswer) => participantAnswer.questionId
            )
        ),
    ];
    const [questionInfos, setQuestionInfos] = useState<Question[]>([]);
    const [openQuestionsModal, setOpenQuestionsModal] = useState(false);

    useEffect(() => {
        const fetchQuestionInfos = () => {
            questionIds.forEach((id) => {
                fetch(QUIZMASTER_QUESTION_GET_QUESTION + `${id}`)
                    .then((response) => response.json())
                    .then((data: Question) =>
                        setQuestionInfos((prev) => [...prev, data])
                    )
                    .catch((err) => {
                        console.log(err);
                    });
            });
        };
        if (questionIds.length !== 0 || opened) {
            fetchQuestionInfos();
        }
    }, []);

    if (!opened) {
        return null;
    }

    return (
        <Modal
            opened={opened}
            size={"xl"}
            onClose={onClose}
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
                <div className="mt-2 mb-8">
                    <p className="text-xs text-gray-900 mb-2">Hosted by:</p>
                    <div className=" bg-gray-800 flex w-fit justify-center items-center px-4 py-2 rounded-md gap-2">
                        <UserIcon width={18} height={18} color="white" />
                        <p className="text-sm text-white w-fit">
                            {sessionReport.hostName}
                        </p>
                    </div>
                </div>
                <div className="flex mb-2 mt-8 justify-between items-end">
                    <p className="text-base text-gray-900  font-semibold">
                        Leader Board
                    </p>
                    <div
                        className="px-4 py-2 cursor-pointer rounded-md hover:bg-slate-50 border border-gray-200"
                        onClick={() => {
                            setOpenQuestionsModal(true);
                        }}
                    >
                        <p className="text-sm font-medium text-gray-950">
                            {`View questions (${questionIds.length})`}
                        </p>
                    </div>
                </div>

                <LeaderBoard
                    participants={sessionReport.leaderboardReports}
                    sessionReport={sessionReport}
                    sessionName={sessionName}
                />
                <div className="flex gap-2 justify-end mt-14">
                    <Button variant="default" onClick={onClose}>
                        Cancel
                    </Button>

                    <Popover
                        width={200}
                        position="bottom"
                        withArrow
                        shadow="md"
                    >
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
                                Export Session
                            </Button>
                        </Popover.Target>
                        <Popover.Dropdown style={{ padding: 0 }}>
                            <div className="overflow-clip bg-white h-fit cursor-pointer text-gray-800 py-2 px-4 flex-col">
                                <CSVLink
                                    className="py-2 px-4 hover:font-medium"
                                    data={answersAllParticipantsToCsvData(
                                        sessionReport
                                    )}
                                    separator=","
                                    filename={`[${sessionName}] Quiz Answers.csv`}
                                >
                                    Export as CSV
                                </CSVLink>
                                <div className="py-2 px-4 hover:font-medium">
                                    <PDFDownloadLink
                                        fileName={`[${sessionName}] - Reports (Host by ${sessionReport.hostName})`}
                                        document={
                                            <QuizSessionReportPDF
                                                sessionName={sessionName}
                                                sessionReport={sessionReport}
                                                questionInfos={questionInfos}
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
                <ViewSessionQuestionsModal
                    opened={openQuestionsModal}
                    onClose={() => setOpenQuestionsModal(false)}
                    sessionName={sessionName}
                    sessionReport={sessionReport}
                    questionInfos={questionInfos}
                />
            </div>
        </Modal>
    );
}
