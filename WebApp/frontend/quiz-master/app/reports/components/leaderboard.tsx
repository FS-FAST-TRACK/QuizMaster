import { EyeIcon } from "@heroicons/react/24/outline";
import { LeaderBoardReport, QuizSessionReport } from "./sessionsReport";
import { Table } from "@mantine/core";
import { useState } from "react";
import ViewParticipantAnswersModal from "@/components/Commons/modals/ViewParticipantAnswersModal";

interface RowProps {
    participant: LeaderBoardReport;
    index: number;
    sessionName: string;
    sessionReport: QuizSessionReport;
}

export default function LeaderBoard({
    participants,
    sessionReport,
    sessionName,
}: {
    participants: LeaderBoardReport[];
    sessionReport: QuizSessionReport;
    sessionName: string;
}) {
    const sortedParticipants = [...participants].sort(
        (a, b) => b.score - a.score
    );

    return (
        <div>
            <div className="border border-gray-300 rounded-md">
                <Table>
                    <Table.Thead>
                        <Table.Tr>
                            <Table.Th>Name</Table.Th>
                            <Table.Th>Score</Table.Th>
                            <Table.Th></Table.Th>
                        </Table.Tr>
                    </Table.Thead>
                    <Table.Tbody>
                        {sortedParticipants.map((participant, index) => (
                            <LeaderBoardRow
                                participant={participant}
                                index={index}
                                sessionReport={sessionReport}
                                sessionName={sessionName}
                                key={index}
                            />
                        ))}
                    </Table.Tbody>
                </Table>
            </div>
        </div>
    );
}

function LeaderBoardRow({
    participant,
    index,
    sessionReport,
    sessionName,
}: RowProps) {
    const [openModal, setOpenModal] = useState(false);
    return (
        <Table.Tr key={participant.id}>
            <Table.Td>{participant.participantName}</Table.Td>
            <Table.Td>
                <p>
                    <b>{participant.score}</b> points
                </p>
            </Table.Td>
            <Table.Td>
                <EyeIcon
                    width={20}
                    height={20}
                    color="rgb(31 41 55)"
                    className="opacity-50 hover:opacity-100 cursor-pointer"
                    onClick={() => setOpenModal(true)}
                />
            </Table.Td>
            <ViewParticipantAnswersModal
                opened={openModal}
                onClose={() => setOpenModal(false)}
                sessionName={sessionName}
                sessionReport={sessionReport}
                participantName={participant.participantName}
                participantScore={participant.score}
                participantPlace={index + 1}
            />
        </Table.Tr>
    );
}
