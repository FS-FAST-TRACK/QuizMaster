import React from "react";
import { Page, Text, Document, StyleSheet, View } from "@react-pdf/renderer";
import { ParticipantAnswer } from "@/components/Commons/modals/ViewParticipantAnswersModal";
import { Question } from "@/lib/definitions";

export function IndividualParticipantAnswersReport({
    participantAnswers,
    questionInfos,
    sessionName,
    sessionDuration,
}: {
    participantAnswers: ParticipantAnswer[];
    questionInfos: Question[];
    sessionName: string;
    sessionDuration: string;
}) {
    return (
        <Document>
            <Page style={styles.body}>
                <View style={styles.header}>
                    <View>
                        <Text style={styles.sessionDuration}>Participant</Text>
                        <Text style={styles.sessionTitle}>
                            {participantAnswers[0]?.participantName}
                        </Text>
                    </View>
                    <View>
                        <Text style={styles.sessionTitle}>{sessionName}</Text>
                        <Text style={styles.sessionDuration}>
                            {sessionDuration}
                        </Text>
                    </View>
                </View>
                <Text style={[styles.sectionTitle]}>Participant Answers</Text>
                <View>
                    <View style={styles.table}>
                        <View style={styles.row}>
                            <Text style={[styles.column1, styles.tableHeader]}>
                                Question ID
                            </Text>
                            <Text style={[styles.column2, styles.tableHeader]}>
                                Question Statement
                            </Text>
                            <Text style={[styles.column3, styles.tableHeader]}>
                                Participant Name
                            </Text>
                            <Text style={[styles.column4, styles.tableHeader]}>
                                Answer
                            </Text>
                        </View>
                        {participantAnswers.map((answer) => {
                            return (
                                <ParticipantAnswersTableRow
                                    participantAnswer={answer}
                                    questionInfos={questionInfos}
                                />
                            );
                        })}
                    </View>
                </View>
            </Page>
        </Document>
    );
}

function ParticipantAnswersTableRow({
    participantAnswer,
    questionInfos,
}: {
    participantAnswer: ParticipantAnswer;
    questionInfos: Question[];
}) {
    const questionStatement = questionInfos.find(
        (q) => q.id === participantAnswer.questionId
    )?.qStatement;
    return (
        <View style={styles.row}>
            <Text style={styles.column1}>{participantAnswer.questionId}</Text>
            <Text style={styles.column2}>{questionStatement}</Text>
            <Text style={styles.column3}>
                {participantAnswer.participantName}
            </Text>
            <Text style={styles.column4}>{participantAnswer.answer}</Text>
        </View>
    );
}

const styles = StyleSheet.create({
    body: {
        paddingTop: 32,
        paddingBottom: 32,
        paddingHorizontal: 32,
    },
    header: {
        flexDirection: "row",
        justifyContent: "space-between",
        marginBottom: 32,
    },
    sessionTitle: {
        fontSize: 20,
        textAlign: "left",
        color: "black",
        marginBottom: 6,
        fontWeight: 600,
    },
    sectionTitle: {
        fontSize: 14,
        marginBottom: 8,
        fontWeight: 600,
    },
    sessionDuration: {
        fontSize: 8,
        color: "rgb(156 163 175)",
        textAlign: "left",
    },
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
    column1: {
        width: "10%",
        height: "100%",
        textAlign: "center",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
    column2: {
        width: "40%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },

    column3: {
        width: "20%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },
    column4: {
        width: "30%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },
});
