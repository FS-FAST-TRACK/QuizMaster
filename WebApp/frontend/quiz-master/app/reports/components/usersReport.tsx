import React, { useState, useEffect } from "react";
import { QuizSessionReportPDF } from "./pdfReports/QuizSessionReportPDF";
import { PDFDownloadLink } from "@react-pdf/renderer";
import { ArrowPathIcon } from "@heroicons/react/24/outline";
import { QUIZMASTER_ACCOUNT_GET } from "@/api/api-routes";
import { UsersTable } from "./usersTable";
import { parseDateStringToDate } from "@/lib/dateTimeUtils";
import { Button, Popover } from "@mantine/core";
import { CSVLink } from "react-csv";
import { usersToCsvData } from "@/lib/csvDataGenerator";
import { UsersReportPDF } from "./pdfReports/UsersReportPDF";

export interface User {
    id: number;
    lastName: string;
    firstName: string;
    email: string;
    userName: string;
    activeData: boolean;
    dateCreated: Date;
    dateUpdated: Date | null;
    updatedByUser: string;
    roles: string[];
}

export default function UserReports() {
    const [users, setUsers] = useState<User[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<any>();

    const fetchUsers = async () => {
        setIsLoading(true);
        try {
            const response = await fetch(QUIZMASTER_ACCOUNT_GET, {
                method: "GET",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
            });
            const data = await response.json();

            const sortedUsers = data.sort(
                (a: User, b: User) =>
                    new Date(b.dateCreated).getTime() -
                    new Date(a.dateCreated).getTime()
            );

            setUsers(sortedUsers as User[]);
            setIsLoading(false);
        } catch (e) {
            setError(e);
            console.log(e);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const errorOverLay = () => {
        return (
            <div className="absolute bg-gray-300/60 top-0 bottom-0 left-0 right-0 flex flex-col items-center gap-4 pt-16">
                <p className="text-red-500 font-semibold text-lg">
                    Something went wrong...
                </p>
                <div className="flex justify-center items-center gap-1 cursor-pointer hover:scale-105">
                    <ArrowPathIcon width={20} height={20} />
                    <p className="underliner">Retry fetching users</p>
                </div>
            </div>
        );
    };

    return (
        <div className="p-4 relative">
            {error && errorOverLay()}
            <div className="flex justify-between mt-4 mb-4">
                <p className="text-2xl font-semibold text-gray-800">Users</p>
                <Popover>
                    <Popover.Target>
                        <Button
                            className="hover:scale-105"
                            style={{ backgroundColor: "var(--primary)" }}
                        >
                            Export
                        </Button>
                    </Popover.Target>
                    <Popover.Dropdown style={{ padding: 0 }}>
                        <div className="overflow-clip bg-white h-fit cursor-pointer text-gray-800 py-2 px-4 flex-col">
                            <CSVLink
                                className="py-2 px-4 hover:font-medium"
                                data={usersToCsvData(users)}
                                separator=","
                                filename={`[USERS] Quiz Master Reports.csv`}
                            >
                                Export as CSV
                            </CSVLink>
                            <div className="py-2 px-4 hover:font-medium">
                                <PDFDownloadLink
                                    fileName={`[USERS] Quiz Master Reports`}
                                    document={<UsersReportPDF users={users} />}
                                >
                                    {({ loading }) =>
                                        loading
                                            ? "Downloading document..."
                                            : "Export as PDF"
                                    }
                                </PDFDownloadLink>
                            </div>
                        </div>
                    </Popover.Dropdown>
                </Popover>
            </div>
            <UsersTable users={users} isLoading={isLoading} />
        </div>
    );
}
