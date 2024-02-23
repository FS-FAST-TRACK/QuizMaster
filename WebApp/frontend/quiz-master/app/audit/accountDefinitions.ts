export const propertyHeadersToSearch = [
    "userAuditTrailId",
    "action",
    "details",
    "userRole",
];

export const selectTypeValues = [
    "Account",
    "Media",
    "Question Difficulty",
    "Question Category",
    "Question Type ",
    "Question",
    "Quiz Set",
    "Quiz Room",
];

// start of action type values
const actionAccountType = [
    "All",
    "User Registration",
    "Partial Registration",
    "Set Admin",
    "User Partial Registration",
    "User Update",
    "Remove Admin",
];

const actionMediaType = ["All", "Upload", "Delete"];

const actionQuizDifficultyType = [
    "All",
    "Delete Difficulty",
    "Update Difficulty",
    "Create Difficulty",
];

const actionQuizCategoryType = [
    "All",
    "Create Category",
    "Update Category",
    "Delete Category",
];

const actionQuizAuditType = [
    "All",
    "Create Quiz Set",
    "Update Quiz Set",
    "Delete Quiz Set",
];

const actionRoomAuditType = [
    "All",
    "Create Room",
    "Update Room",
    "Delete Room",
];

export const actionValues = (value: string | null) => {
    switch (value) {
        case "Media":
            return actionMediaType;
        case "Quiz Difficulty":
            return actionQuizDifficultyType;

        case "Quiz Category":
            return actionQuizCategoryType;
        case "Quiz Set Audit":
            return actionQuizAuditType;
        case "Room Audit":
            return actionRoomAuditType;
        default:
            return actionAccountType;
    }
};
// end
