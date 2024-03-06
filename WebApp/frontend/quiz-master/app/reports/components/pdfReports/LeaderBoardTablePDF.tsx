import { View, StyleSheet, Text } from "@react-pdf/renderer";
import { LeaderBoardReport, QuizSessionReport } from "../sessionsReport";

export function LeaderBoardTablePDF({
    participants,
}: {
    participants: LeaderBoardReport[];
}) {
    const sortedParticipants = [...participants].sort(
        (a, b) => b.score - a.score
    );
    const topThree = sortedParticipants.slice(0, 3);
    const restParticipants = sortedParticipants.slice(3);

    return (
        <View>
            <Text style={styles.sectionTitle}>Leader Board</Text>
            <View style={styles.table}>
                <View style={styles.row}>
                    <Text style={[styles.column1, styles.tableHeader]}>
                        Place
                    </Text>
                    <Text style={[styles.column2, styles.tableHeader]}>
                        Participant Name
                    </Text>
                    <Text style={[styles.column3, styles.tableHeader]}>
                        Score
                    </Text>
                </View>
                {topThree.map((participant, index) => (
                    <LeaderBoardPdfRow
                        participant={participant}
                        index={index + 1}
                    />
                ))}
            </View>

            <Text style={styles.sectionTitle}>Participants</Text>
            <View style={styles.table}>
                <View style={styles.row}>
                    <Text style={[styles.column1, styles.tableHeader]}>
                        Place
                    </Text>
                    <Text style={[styles.column2, styles.tableHeader]}>
                        Participant Name
                    </Text>
                    <Text style={[styles.column3, styles.tableHeader]}>
                        Score
                    </Text>
                </View>
                {restParticipants.map((participant, index) => (
                    <LeaderBoardPdfRow
                        participant={participant}
                        index={index + 4}
                    />
                ))}
            </View>
        </View>
    );
}

function LeaderBoardPdfRow({
    participant,
    index,
}: {
    participant: LeaderBoardReport;
    index: Number;
}) {
    return (
        <View style={styles.row}>
            <Text style={styles.column1}>{index.toString()}</Text>
            <Text style={styles.column2}>{participant.participantName}</Text>
            <Text style={styles.column3}>{participant.score} points</Text>
        </View>
    );
}

const styles = StyleSheet.create({
    row: {
        flexDirection: "row",
        alignItems: "center",
    },
    tableHeader: {
        borderTopWidth: 0,
        fontWeight: 700,
        paddingVertical: 8,
        backgroundColor: "#17a14b",
        color: "white",
    },
    table: {
        borderWidth: 1,
        borderTopWidth: 1,
        borderTopColor: "#17a14b",
        borderColor: "#D1D5DB",
        borderRadius: 6,
        overflow: "hidden",
        marginBottom: 32,
    },
    sectionTitle: {
        fontSize: 12,
        marginBottom: 8,
        fontWeight: 600,
    },
    column1: {
        width: "25%",
        textAlign: "center",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
    column2: {
        width: "50%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderRightWidth: 1,
        borderColor: "#D1D5DB",
    },
    column3: {
        width: "25%",
        textAlign: "center",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
});
