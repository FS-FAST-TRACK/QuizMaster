import { View, StyleSheet, Text } from "@react-pdf/renderer";
import { ParticipantAnswerReport } from "../sessionsReport";

export function ParticipantAnswersTablePDF({
    participantAnswers,
}: {
    participantAnswers: ParticipantAnswerReport[];
}) {
    return (
        <View>
            <View style={styles.table}>
                <View style={styles.row}>
                    <Text style={[styles.column1, styles.tableHeader]}>
                        Question ID
                    </Text>
                    <Text style={[styles.column2, styles.tableHeader]}>
                        Participant Name
                    </Text>
                    <Text style={[styles.column3, styles.tableHeader]}>
                        Answer
                    </Text>
                </View>
                {participantAnswers.map((answer, index) => {
                    return (
                        <ParticipantAnswersTableRow
                            participantAnswer={answer}
                            key={index}
                        />
                    );
                })}
            </View>
        </View>
    );
}

function ParticipantAnswersTableRow({
    participantAnswer,
}: {
    participantAnswer: ParticipantAnswerReport;
}) {
    return (
        <View style={styles.row}>
            <Text style={styles.column1}>{participantAnswer.questionId}</Text>
            <Text style={styles.column2}>
                {participantAnswer.participantName}
            </Text>
            <Text style={styles.column3}>{participantAnswer.answer}</Text>
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
        fontSize: 10,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
    column2: {
        width: "50%",
        fontSize: 10,
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
        fontSize: 10,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
});
