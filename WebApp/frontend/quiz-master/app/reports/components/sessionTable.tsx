import React, { useEffect, useState } from "react";
import { Popover, Table } from "@mantine/core";
import LoadingTable from "./loadingTable";
import { QuizSessionReport } from "./sessionsReport";
import { ArrowDownCircleIcon, EyeIcon } from "@heroicons/react/24/outline";
import TimeDateDuration from "./timeDateDuration";
import { QUIZMASTER_QUIZROOM_GET_BY_ID } from "@/api/api-routes";
import ViewQuizSessionModal from "@/components/Commons/modals/ViewQuizSessionModal";
import { answersAllParticipantsToCsvData } from "@/lib/csvDataGenerator";
import { PDFDownloadLink } from "@react-pdf/renderer";
import { QuizSessionReportPDF } from "./pdfReports/QuizSessionReportPDF";
import { CSVLink } from "react-csv";

const TABLE_COLUMNS = [
    "Session Name",
    "Host",
    "Date",
    "Room Pin",
    "Sets",
    "Participants",
    "",
    "",
];

export default function SessionTable({
    quizReports = [],
    isLoading,
    error,
}: {
    quizReports: QuizSessionReport[];
    isLoading: boolean;
    error: any;
}) {
    const rows = quizReports.map((report, index) => (
        <SessionTableRow session={report} key={index} />
    ));

    if (isLoading) {
        return <LoadingTable columnNames={TABLE_COLUMNS} numberOfRows={30} />;
    }

    if (error) {
        return (
            <div>
                <p className="bg-red-600">Something went wrong...</p>
            </div>
        );
    }

    return (
        <div className="border border-gray-300 rounded-md">
            <Table>
                <Table.Thead>
                    <Table.Tr>
                        <Table.Th>Session Name</Table.Th>
                        <Table.Th>Host</Table.Th>
                        <Table.Th>Date</Table.Th>
                        <Table.Th>Room Pin</Table.Th>
                        <Table.Th>Sets</Table.Th>
                        <Table.Th>Participants</Table.Th>
                        <Table.Th></Table.Th>
                        <Table.Th></Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>{rows}</Table.Tbody>
            </Table>
        </div>
    );
}

export function SessionTableRow({ session }: { session: QuizSessionReport }) {
    const [qRoomDesc, setQRoomDesc] = useState<string>("-- No name--");
    const [qRoomPin, setQRoomPin] = useState<number>(0);
    const [qSets, setQSets] = useState<number>(0);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [openModal, setOpenModal] = useState(false);

    useEffect(() => {
        const fetchSessionReport = async () => {
            setIsLoading(true);
            try {
                const response = await fetch(
                    QUIZMASTER_QUIZROOM_GET_BY_ID(session.roomId) +
                        "?isActive=false",
                    {
                        method: "GET",
                        headers: { "Content-Type": "application/json" },
                        credentials: "include",
                    }
                );
                const responseData = await response.json();
                if (responseData.data) {
                    setQRoomDesc(responseData.data.qRoomDesc);
                    setQRoomPin(responseData.data.qRoomPin);
                    setQSets(responseData.data.set.length);
                }
            } catch (err) {
                console.log(err);
            } finally {
                setIsLoading(false);
            }
        };
        fetchSessionReport();
    }, []);

    return (
        <Table.Tr key={session.id} className="hover:bg-gray-100 cursor-pointer">
            <Table.Td>
                <p className="font-semibold">{qRoomDesc}</p>
            </Table.Td>
            <Table.Td>{session.hostName}</Table.Td>
            <Table.Td>
                <TimeDateDuration
                    startTime={session.startTime}
                    endTime={session.endTime}
                />
            </Table.Td>
            <Table.Td>
                <div>{qRoomPin}</div>
            </Table.Td>
            <Table.Td>{qSets}</Table.Td>
            <Table.Td>{session.noOfParticipants}</Table.Td>
            <Table.Td>
                {
                    <EyeIcon
                        width={24}
                        height={24}
                        color="rgb(31 41 55)"
                        onClick={() => setOpenModal(true)}
                        className="opacity-50 hover:opacity-100 cursor-pointer"
                    />
                }
            </Table.Td>
            <Table.Td>
                {
                    <Popover withArrow arrowPosition="side" shadow="md">
                        <Popover.Target>
                            <ArrowDownCircleIcon
                                width={24}
                                height={24}
                                color="rgb(31 41 55)"
                                onClick={() => alert("Downloading reports...")}
                                className="opacity-50 hover:opacity-100 cursor-pointer"
                            />
                        </Popover.Target>
                        <Popover.Dropdown style={{ padding: 0 }}>
                            <div className="overflow-clip bg-white h-fit cursor-pointer text-gray-800 py-2 px-4 flex-col">
                                <CSVLink
                                    className="py-2 px-4 hover:font-medium"
                                    data={answersAllParticipantsToCsvData(
                                        session
                                    )}
                                    separator=","
                                    filename={`[${qRoomDesc}] Quiz Answers.csv`}
                                >
                                    Export
                                </CSVLink>
                                <div className="py-2 px-4 hover:font-medium">
                                    <PDFDownloadLink
                                        fileName={`[${qRoomDesc}] - Reports (Host by ${session.hostName})`}
                                        document={
                                            <QuizSessionReportPDF
                                                sessionName={qRoomDesc}
                                                sessionReport={session}
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
                }
            </Table.Td>
            <ViewQuizSessionModal
                sessionReport={session}
                opened={openModal}
                onClose={() => setOpenModal(false)}
                sessionName={qRoomDesc}
            />
        </Table.Tr>
    );
}
