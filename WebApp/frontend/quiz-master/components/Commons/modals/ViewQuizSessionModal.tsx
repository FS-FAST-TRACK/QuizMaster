import LeaderBoard from "@/app/reports/components/leaderboard";
import { QuizSessionReportPDF } from "@/app/reports/components/pdfReports/QuizSessionReportPDF";
import { QuizSessionReport } from "@/app/reports/components/sessionsReport";
import { answersAllParticipantsToCsvData } from "@/lib/csvDataGenerator";
import { formatDateTimeRange } from "@/lib/dateTimeUtils";
import { UserIcon, ChevronDownIcon } from "@heroicons/react/24/outline";
import { Button, Modal, ModalHeader, Popover } from "@mantine/core";
import { PDFDownloadLink } from "@react-pdf/renderer";
import { CSVLink } from "react-csv";

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
                <p className="text-base text-gray-900 mb-2 font-semibold mt-8">
                    Leader Board
                </p>
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
                                rightSection={
                                    <ChevronDownIcon width={16} height={16} />
                                }
                                color="var(--primary)"
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
            </div>
        </Modal>
    );
}
