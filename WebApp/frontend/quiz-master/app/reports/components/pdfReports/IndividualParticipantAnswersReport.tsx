import React from "react";
import { Page, Text, Document, StyleSheet, View } from "@react-pdf/renderer";
import { ParticipantAnswer } from "@/components/Commons/modals/ViewParticipantAnswersModal";
import { Question } from "@/lib/definitions";
import { truncateString } from "@/lib/stringUtils";

export function IndividualParticipantAnswersReport({
    participantAnswers,
    questionInfos,
    sessionName,
    sessionDuration,
    participantScore,
}: {
    participantAnswers: ParticipantAnswer[];
    questionInfos: Question[];
    sessionName: string;
    sessionDuration: string;
    participantScore: number;
}) {
    return (
        <Document>
            <Page style={styles.body}>
                <View style={styles.header}>
                    <View>
                        <Text style={styles.sessionDuration}>Participant</Text>
                        <View
                            style={{
                                flexDirection: "row",
                                alignItems: "flex-end",
                                gap: 12,
                            }}
                        >
                            <Text style={styles.participantName}>
                                {participantAnswers[0]?.participantName.toUpperCase()}
                            </Text>
                            <Text style={{ color: "grey", fontSize: 12 }}>
                                {participantScore} points
                            </Text>
                        </View>
                    </View>
                    <View>
                        <Text style={styles.sectionTitle}>
                            {truncateString(sessionName, 30)}
                        </Text>
                        <Text style={styles.sessionDuration}>
                            {sessionDuration}
                        </Text>
                    </View>
                </View>
                <Text style={[styles.sectionTitle]}>Participant Answers</Text>
                <View>
                    <View style={styles.table}>
                        <View style={styles.row}>
                            <Text
                                style={[
                                    styles.questionStatementColumn,
                                    styles.tableHeader,
                                ]}
                            >
                                Question Statement
                            </Text>
                            <Text
                                style={[
                                    styles.answerColumn,
                                    styles.tableHeader,
                                ]}
                            >
                                Answer
                            </Text>
                            <Text
                                style={[
                                    styles.correctAnswer,
                                    styles.tableHeader,
                                ]}
                            >
                                Correct Answer
                            </Text>
                        </View>
                        {participantAnswers.map((answer, index) => {
                            return (
                                <ParticipantAnswersTableRow
                                    participantAnswer={answer}
                                    questionInfos={questionInfos}
                                    key={index}
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

    const correctAnswer =
        questionInfos
            .find((q) => q.id === participantAnswer.questionId)
            ?.details.find((detail) => detail.detailTypes.includes("answer"))
            ?.qDetailDesc || "";

    return (
        <View style={styles.row}>
            <Text style={styles.questionStatementColumn}>
                {questionStatement}
            </Text>
            {participantAnswer.answer ? (
                <Text style={styles.answerColumn}>
                    {participantAnswer.answer}
                </Text>
            ) : (
                <Text style={[styles.answerColumn, { color: "grey" }]}>
                    {"<No answer submitted>"}
                </Text>
            )}

            <Text style={styles.correctAnswer}>{correctAnswer}</Text>
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
    participantName: {
        fontSize: 20,
        textAlign: "left",
        color: "black",
        fontWeight: 600,
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
    questionStatementColumn: {
        width: "50%",
        height: "100%",
        textAlign: "center",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderColor: "#D1D5DB",
    },
    answerColumn: {
        width: "25%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },

    correctAnswer: {
        width: "25%",
        height: "100%",
        fontSize: 8,
        paddingVertical: 8,
        paddingHorizontal: 8,
        borderTopWidth: 1,
        borderLeftWidth: 1,
        borderColor: "#D1D5DB",
    },
});
