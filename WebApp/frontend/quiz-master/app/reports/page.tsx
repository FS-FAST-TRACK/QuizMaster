"use client";

import { Tabs } from "@mantine/core";
import QuestionReport from "./components/questionsReport";
import SessionReports from "./components/sessionsReport";
import UserReports from "./components/usersReport";

export default function Page() {
    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow">
            <QuestionReport />
            <div className="flex flex-col p-8 gap-4 bg-white rounded-lg border shadow-lg">
                <Tabs defaultValue={"sessions"}>
                    <Tabs.List>
                        <Tabs.Tab value="sessions">Sessions</Tabs.Tab>
                        <Tabs.Tab value="users">Users</Tabs.Tab>
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
