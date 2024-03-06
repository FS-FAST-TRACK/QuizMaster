"use client";

import { Tabs } from "@mantine/core";
import QuestionReport from "./components/questionsReport";
import SessionReports from "./components/sessionsReport";
import UserReports from "./components/usersReport";
import { useState } from "react";

export default function Page() {
    const [activeTab, setActiveTab] = useState<string | null>("sessions");

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow overflow-scroll">
            <div className="flex flex-col px-4 py-2 gap-4 bg-white rounded-lg border shadow-lg overflow-scroll min-h-screen">
                <Tabs value={activeTab} onChange={setActiveTab}>
                    <Tabs.List>
                        <Tabs.Tab value="sessions" color="var(--primary)">
                            <p
                                className={`text-base ${
                                    activeTab === "sessions" &&
                                    "font-semibold text-green-600"
                                }`}
                            >
                                Sessions
                            </p>
                        </Tabs.Tab>
                        <Tabs.Tab value="users" color="var(--primary)">
                            <p
                                className={`text-base ${
                                    activeTab === "users" &&
                                    "font-semibold text-green-600"
                                }`}
                            >
                                Users
                            </p>
                        </Tabs.Tab>
                    </Tabs.List>
                    <Tabs.Panel value="sessions">
                        <SessionReports />
                    </Tabs.Panel>
                    <Tabs.Panel value="users">
                        <UserReports />
                    </Tabs.Panel>
                </Tabs>
            </div>
        </div>
    );
}
