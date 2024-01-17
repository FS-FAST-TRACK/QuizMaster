import { QUIZMASTER_MONITORING_AUDIT_GET } from "@/api/api-routes";

export async function fetchAudit(uri: string) {
    try {
        var apiUrl = `${uri}`;

        const response = await fetch(apiUrl);

        if (!response.ok) {
            throw new Error(`Failed to fetch data. Status: ${response.status}`);
        }

        const data = await response.json();

        return { data };
    } catch (error) {
        console.error("Error fetching user audit data:", error);
        throw error;
    }
}
