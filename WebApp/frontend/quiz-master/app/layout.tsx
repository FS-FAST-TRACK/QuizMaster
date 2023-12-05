import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import "@mantine/core/styles.css";
import {
    MantineProvider,
    createTheme,
    ColorSchemeScript,
    TextInput,
} from "@mantine/core";
import { AuthProvider } from "./providers";
import { getServerSession } from "next-auth";
import toast, { Toaster } from "react-hot-toast";
import { Notifications } from "@mantine/notifications";
const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
    title: "QuizMaster",
    description: "QuizMaster",
};

export default async function RootLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    const session = await getServerSession();
    console.log(session);
    return (
        <html lang="en">
            <head>
                <ColorSchemeScript />
                <link rel="shortcut icon" href="/favicon.svg" />
                <meta
                    name="viewport"
                    content="minimum-scale=1, initial-scale=1, width=device-width, user-scalable=no"
                />
            </head>
            <body className={inter.className} style={{ overflow: "hidden" }}>
                <MantineProvider>
                    <Toaster />
                    <Notifications
                        className="absolute top-1 right-5"
                        position="top-right"
                    />
                    <AuthProvider session={session}>{children}</AuthProvider>{" "}
                </MantineProvider>
            </body>
        </html>
    );
}
