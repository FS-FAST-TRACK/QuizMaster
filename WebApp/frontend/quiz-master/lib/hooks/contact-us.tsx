import { QUIZMASTER_SYSTEM_POST_CONTACT_INFO, QUIZMASTER_SYSTEM_POST_REACH_OUT, QUIZMASTER_SYSTEM_POST_REVIEW } from "@/api/api-routes";
import { ContactDetails, ContactUsCreateValues, Feedback } from "../definitions";

export async function postContactUs({
    contactForm, 
}: {
    contactForm:ContactUsCreateValues
}) {
    try {
        const token = localStorage.getItem("token"); //just temporary
        // Post Question
        const res = await fetch(`${QUIZMASTER_SYSTEM_POST_REACH_OUT}`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(contactForm),
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await res.json();
        if (res.status === 200) {
            return data;
        } else {
            throw new Error("Failed to update contact info");
        }
    } catch (error) {
        throw new Error("Failed to update contact info.");
    }
}

export async function UpdateContactDetails({
    contactForm,
}: {
    contactForm: ContactDetails;
}) {
    try {
        const token = localStorage.getItem("token"); //just temporary
        // Post Question
        const res = await fetch(`${QUIZMASTER_SYSTEM_POST_CONTACT_INFO}`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(contactForm),
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        });
        const data = await res.json();
        if (res.status === 200) {
            return data;
        } else {
            throw new Error("Failed to update contact info");
        }
    } catch (error) {
        throw new Error("Failed to update contact info.");
    }
}

export async function postReachOut({
    feedbackForm, 
}: {
    feedbackForm:Feedback
}) {
    try {
        // Post Question
        const res = await fetch(`${QUIZMASTER_SYSTEM_POST_REVIEW}`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(feedbackForm),
            headers: {
                "Content-Type": "application/json",
            },
        });
        const data = await res.json();
        if (res.status === 200) {
            return data;
        } else {
            throw new Error("Failed to send review");
        }
    } catch (error) {
        throw new Error("Failed to send review.");
    }
}