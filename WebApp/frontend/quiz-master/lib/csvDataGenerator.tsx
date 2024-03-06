import {
    QUIZMASTER_QCATEGORY_GET_CATEGORY,
    QUIZMASTER_QDIFFICULTY_GET_DIFFICULTY,
    QUIZMASTER_QTYPE_GET_TYPE,
    QUIZMASTER_QUESTION_GET_QUESTION,
} from "@/api/api-routes";
import {
    Question,
    QuestionCategory,
    QuestionDetail,
    QuestionDifficulty,
    QuestionType,
} from "@/lib/definitions";
import { QuizSessionReport } from "@/app/reports/components/sessionsReport";
import { User } from "@/app/reports/components/usersReport";

export interface ParticipantAnswersData {
    sessionName: string;
    sessionReport: QuizSessionReport;
    participantName: string;
    participantScore: number;
    participantPlace: number;
}
export function participantAnswersToCsvData(
    participantAnswersData: ParticipantAnswersData,
    questionInfos: Question[]
) {
    const answers =
        participantAnswersData.sessionReport.participantAnswerReports.filter(
            (report) =>
                report.participantName ===
                participantAnswersData.participantName
        );
    const csvData = [
        [
            "Session Id",
            "Participant Name",
            "Question ID",
            "Statement",
            "Answer",
        ],
    ];

    answers.forEach((report) => {
        const sessionId = report.sessionId;
        const participantName = report.participantName;
        const questionId = report.questionId.toString();
        const questionStatement = questionInfos.find(
            (q) => q.id === report.questionId
        )?.qStatement;
        const answer = report.answer;

        csvData.push([
            sessionId,
            participantName,
            questionId,
            questionStatement || "",
            answer,
        ]);
    });

    return csvData;
}

export function answersAllParticipantsToCsvData(
    sessionReport: QuizSessionReport
) {
    const participants = [
        ...new Set(
            sessionReport.participantAnswerReports.map(
                (answerReport) => answerReport.participantName
            )
        ),
    ];

    const questions = [
        ...new Set(
            sessionReport.participantAnswerReports.map(
                (question) => question.questionId
            )
        ),
    ];

    const csvData = [["Question", ...participants]];

    questions.forEach((question) => {
        const row: string[] = [question.toString()];
        const answersByQuestionId =
            sessionReport.participantAnswerReports.filter(
                (answerReport) => answerReport.questionId === question
            );

        participants.forEach((participant) => {
            const participantAnswer = answersByQuestionId.find(
                (answer) => answer.participantName === participant
            );
            row.push(
                participantAnswer
                    ? participantAnswer.answer
                    : "--No answer submitted--"
            );
        });
        csvData.push(row);
    });
    return csvData;
}

export function usersToCsvData(users: User[]) {
    const userCsvData = users;

    return userCsvData;
}
