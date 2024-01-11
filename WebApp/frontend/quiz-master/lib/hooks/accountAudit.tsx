import { QUIZMASTER_MONITORING_USER_AUDIT_GET } from "@/api/api-routes";

export async function fetchUserAudit() {
    try {
        var apiUrl = `https://localhost:7065/api/audit/user/all`;

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
