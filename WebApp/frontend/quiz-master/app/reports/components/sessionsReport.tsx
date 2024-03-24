import React, { useEffect, useState } from "react";
import SessionTable from "./sessionTable";
import { QUIZMASTER_SYSTEM_GET_QUIZ_REPORTS } from "@/api/api-routes";

export interface QuizSessionReport {
    id: number;
    hostId: number;
    hostName: string;
    startTime: Date;
    endTime: Date;
    noOfParticipants: number;
    roomId: number;
    participantAnswerReports: ParticipantAnswerReport[];
    leaderboardReports: LeaderBoardReport[];
}

export interface LeaderBoardReport {
    id: number;
    participantName: string;
    score: number;
    sessionId: string;
}

export interface ParticipantAnswerReport {
    id: number;
    sessionId: string;
    participantName: string;
    answer: string;
    questionId: number;
    screenshotLink: string;
    score: number;
    points: number;
}

export default function SessionReports() {
    const [quizReports, setQuizReports] = useState<QuizSessionReport[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState();

    useEffect(() => {
        const fetchSessionReports = async () => {
            setIsLoading(true);
            try {
                const response = await fetch(
                    QUIZMASTER_SYSTEM_GET_QUIZ_REPORTS,
                    {
                        method: "GET",
                        headers: { "Content-Type": "application/json" },
                        credentials: "include",
                    }
                );
                const responseData = await response.json();
                // Sort quiz reports by startTime in descending order
                const sortedQuizReports = responseData.data.sort(
                    (a: QuizSessionReport, b: QuizSessionReport) => {
                        return (
                            new Date(b.startTime).getTime() -
                            new Date(a.startTime).getTime()
                        );
                    }
                );

                setQuizReports(sortedQuizReports as QuizSessionReport[]);
            } catch (e: any) {
                setError(e);
            } finally {
                setIsLoading(false);
            }
        };
        fetchSessionReports();
    }, []);

    return (
        <div className="p-4">
            <h1 className="text-2xl font-semibold text-gray-800 mt-4 mb-4">
                Quiz Sessions
            </h1>
            <SessionTable
                isLoading={isLoading}
                quizReports={quizReports}
                error={false}
            />
        </div>
    );
}
