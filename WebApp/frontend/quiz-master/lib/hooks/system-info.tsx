import { QUIZMASTER_SET_POST, QUIZMASTER_SET_PUT } from "@/api/api-routes";
import { ContactDetails, ContactUsCreateValues, QuestionSetDTO, SetDTO, SystemInfoDto } from "../definitions";

export async function postSystemInfo({
    systemDetails,
}: {
    systemDetails: SystemInfoDto;
}) {
    try {
        console.log(systemDetails);
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}

export async function UpdateSystemInfo({
    systemDetails,
}: {
    systemDetails: SystemInfoDto;
}) {
    try {
        console.log(systemDetails);
    } catch (error) {
        throw new Error("Failed to create question.");
    }
}
