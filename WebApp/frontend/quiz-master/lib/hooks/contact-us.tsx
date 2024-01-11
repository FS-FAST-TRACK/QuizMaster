import { QUIZMASTER_SET_POST, QUIZMASTER_SET_PUT } from "@/api/api-routes";
import { ContactDetails, ContactUsCreateValues, QuestionSetDTO, SetDTO } from "../definitions";

export async function postContactUs({
    contactForm,
}: {
    contactForm: ContactUsCreateValues;
}) {
    try {
        console.log(contactForm);
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}

export async function UpdateContactDetails({
    contactForm,
}: {
    contactForm: ContactDetails;
}) {
    try {
        console.log(contactForm);
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}
