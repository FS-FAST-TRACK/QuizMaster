import React from "react";
import { Page, Text, Document, StyleSheet, View } from "@react-pdf/renderer";
import { QuizSessionReport } from "../sessionsReport";
import { formatDateTimeRange } from "@/lib/dateTimeUtils";
import { LeaderBoardTablePDF } from "./LeaderBoardTablePDF";
import { ParticipantAnswersTablePDF } from "./ParticipantAnswersTablePDF";

export function QuizSessionReportPDF({
    sessionName,
    sessionReport,
}: {
    sessionName: string;
    sessionReport: QuizSessionReport;
}) {
    return (
        <Document>
            <Page style={styles.body}>
                <View style={styles.header}>
                    <View>
                        <Text style={styles.sessionTitle}>{sessionName}</Text>
                        <Text style={styles.sessionDuration}>
                            {formatDateTimeRange(
                                sessionReport.startTime,
                                sessionReport.endTime
                            )}
                            {` • ${sessionReport.noOfParticipants} participant(s)`}
                        </Text>
                    </View>
                </View>
                <Text
                    style={{
                        marginTop: 32,
                        fontSize: 12,
                        fontWeight: 400,
                        color: "rgb(31 41 55)",
                    }}
                >
                    Hosted
                </Text>
                <Text style={styles.hostContainer}>
                    {sessionReport.hostName}
                </Text>
                <View style={{ marginTop: 36 }}>
                    <LeaderBoardTablePDF
                        participants={sessionReport.leaderboardReports}
                    />
                </View>
            </Page>
            <Page style={styles.body}>
                <Text style={styles.sectionTitle}>Participants Answers</Text>
                <ParticipantAnswersTablePDF
                    participantAnswers={sessionReport.participantAnswerReports}
                />
            </Page>
        </Document>
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
    text: {
        margin: 12,
        fontSize: 14,
        textAlign: "justify",
        fontFamily: "Times-Roman",
    },
    image: {
        height: 300,
    },

    hostContainer: {
        backgroundColor: "rgb(31 41 55)",
        paddingHorizontal: 16,
        paddingVertical: 8,
        marginTop: 4,
        borderRadius: 4,
        alignSelf: "flex-start",
        color: "white",
        fontSize: 12,
    },

    pageNumber: {
        position: "absolute",
        fontSize: 12,
        bottom: 30,
        left: 0,
        right: 0,
        textAlign: "center",
        color: "grey",
    },
});
