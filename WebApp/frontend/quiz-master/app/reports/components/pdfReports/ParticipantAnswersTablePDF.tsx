import { View, StyleSheet, Text, Font } from "@react-pdf/renderer";
import { ParticipantAnswerReport } from "../sessionsReport";
import { Question } from "@/lib/definitions";

Font.register({
    family: "Open Sans",
    fonts: [
        {
            src: "https://cdn.jsdelivr.net/npm/open-sans-all@0.1.3/fonts/open-sans-regular.ttf",
        },
        {
            src: "https://cdn.jsdelivr.net/npm/open-sans-all@0.1.3/fonts/open-sans-600.ttf",
            fontWeight: 600,
        },
        {
            src: "https://cdn.jsdelivr.net/npm/open-sans-all@0.1.3/fonts/open-sans-700.ttf",
            fontWeight: 700,
        },
    ],
});

export function ParticipantAnswersTablePDF({
    participantAnswers,
    questionInfos,
}: {
    participantAnswers: ParticipantAnswerReport[];
    questionInfos: Question[];
}) {
    return (
        <View>
            <View>
                {questionInfos.map((info, index) => {
                    return (
                        <QuestionCorrectResponses
                            key={index}
                            qInfo={info}
                            participantAnswers={participantAnswers}
                        />
                    );
                })}
            </View>
        </View>
    );
}

function QuestionCorrectResponses({
    qInfo,
    participantAnswers,
}: {
    qInfo: Question;
    participantAnswers: ParticipantAnswerReport[];
}) {
    return (
        <View style={styles.questionContainer}>
            <Text
                style={styles.qStatement}
            >{`Question: ${qInfo.qStatement}`}</Text>
            <View
                style={{
                    flexDirection: "row",
                    alignItems: "center",
                    gap: 8,
                    marginBottom: 24,
                }}
            >
                <Text style={styles.text}>Answer: </Text>
                <Text style={styles.qAnswer}>
                    {
                        qInfo.details.find((d) =>
                            d.detailTypes.includes("answer")
                        )?.qDetailDesc
                    }
                </Text>
            </View>
            <View style={{ gap: 12 }}>
                {participantAnswers
                    .filter(
                        (answer: ParticipantAnswerReport) =>
                            answer.questionId === qInfo.id
                    )
                    .map((answer: ParticipantAnswerReport, index) => {
                        return (
                            <View
                                key={index}
                                style={{
                                    flexDirection: "row",
                                    alignItems: "center",
                                    gap: 8,
                                }}
                            >
                                <View
                                    style={{
                                        flexDirection: "row",
                                        alignItems: "center",
                                        gap: 4,
                                    }}
                                >
                                    <Text
                                        style={styles.qUserName}
                                    >{`${answer.participantName}`}</Text>
                                    <Text style={styles.text}>
                                        {" answered:  "}
                                    </Text>
                                    {answer.answer ? (
                                        <Text style={styles.qParticipantAnswer}>
                                            {answer.answer}
                                        </Text>
                                    ) : (
                                        <Text style={styles.qNoAnswer}>
                                            {"-- No answer submitted --"}
                                        </Text>
                                    )}
                                </View>
                            </View>
                        );
                    })}
            </View>
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
    questionContainer: {
        padding: 16,
        borderColor: "#D1D5DB",
        borderWidth: 1,
        borderRadius: 6,
        marginBottom: 32,
    },
    sectionTitle: {
        fontSize: 12,
        marginBottom: 8,
        fontWeight: 600,
    },
    text: {
        fontFamily: "Open Sans",
        fontSize: 8,
    },
    qStatement: {
        fontFamily: "Open Sans",
        fontSize: 12,
        fontWeight: 700,
        marginBottom: 16,
    },
    qAnswer: {
        fontFamily: "Open Sans",
        fontSize: 10,
        color: "#17A14B",
        fontWeight: 700,
    },
    qUserName: {
        fontFamily: "Open Sans",
        fontSize: 8,
        fontWeight: 600,
        color: "white",
        paddingHorizontal: 4,
        paddingVertical: 2,
        backgroundColor: "#3C3C3C",
        borderRadius: 2,
    },
    qParticipantAnswer: {
        fontFamily: "Open Sans",
        fontSize: 12,
        fontWeight: 600,
    },
    qNoAnswer: {
        color: "grey",
        fontSize: 8,
    },
});
