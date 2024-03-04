import { QUIZMASTER_ACCOUNT } from '@/api/api-routes'

export async function GetAllUsers() 
{
    try {
        const token = localStorage.getItem("token"); //just temporary
        // Post Question
        const res = await fetch(`${QUIZMASTER_ACCOUNT}`, {
            method: "GET",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await res.json();
        console.log("DATAAA:",data);
        if (res.status === 200) {
            return data;
        }
    } catch (error) {
        throw new Error("Error getting all users: "+error);
    }
}
