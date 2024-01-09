"use client";

import ErrorContainer from "@/components/pages/ErrorContainer";

export default function Layout({ children }: { children: React.ReactNode }) {
    return <ErrorContainer>{children}</ErrorContainer>;
}
