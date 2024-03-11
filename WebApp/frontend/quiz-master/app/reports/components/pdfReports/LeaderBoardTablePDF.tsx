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

    return (
        <View>
            <Text style={styles.sectionTitle}>Leader Board</Text>
            <View style={styles.table}>
                <View style={styles.row}>
                    <Text style={[styles.column1, styles.tableHeader]}>
                        Participant Name
                    </Text>
                    <Text style={[styles.column2, styles.tableHeader]}>
                        Score
                    </Text>
                </View>
                {sortedParticipants.map((participant, index) => (
                    <LeaderBoardPdfRow
                        participant={participant}
                        index={index + 1}
                        key={index}
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
            <Text style={styles.column1}>{participant.participantName}</Text>
            <Text style={styles.column2}>{participant.score} points</Text>
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
        width: "50%",
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
});
