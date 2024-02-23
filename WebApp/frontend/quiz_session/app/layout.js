import { Inter } from "next/font/google";
import "./globals.css";
import { MantineProvider, ColorSchemeScript } from "@mantine/core";
import "@mantine/core/styles.css";
import { Notifications } from "@mantine/notifications";

const inter = Inter({ subsets: ["latin"] });

export const metadata = {
  title: "FullScale - QuizMaster",
  description:
    "To ignite the spirit of friendly competition, knowledge exploration, and personal growth through the Quiz Bee Competition Web and Mobile Application, creating a community of lifelong learners and champions.",
};

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <head>
        <ColorSchemeScript />
      </head>
      <body
        className={`${inter.className} w-screen h-screen bg-wall overflow-x-hidden`}
      >
        <MantineProvider withGlobalSyles withNormalizeCss>
          <Notifications
            position="top-right"
            zIndex={1000}
            className="absolute top-4 right-4"
          />
          {children}
        </MantineProvider>
      </body>
    </html>
  );
}
