export interface InputValidator {
    Title?: string;
    Regex: RegExp;
    ErrorMessage: string;
}
export const isRequired: InputValidator = {
    Regex: new RegExp(".+"),
    ErrorMessage: "Required. ",
};

export const mustBeEmail: InputValidator = {
    Title: "Email Checker Failed",
    Regex: new RegExp("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"),
    ErrorMessage: "Not a valid email. ",
};

export const mustBeNumber: InputValidator = {
    Regex: new RegExp("^[0-9]+$"),
    ErrorMessage: "Not a valid number. ",
};

export const userNameValidator: InputValidator = {
    Title: "Email Checker Failed",
    Regex: new RegExp("^[a-zA-Z0-9]+$"),
    ErrorMessage:
        "Not a valid username. Must only be alphanumerics and not empty. ",
};

export const mustHaveDigit: InputValidator = {
    Title: "Character not found",
    Regex: new RegExp(".*[0-9].*"),
    ErrorMessage: "Must have one numeric value. ",
};

export const mustHaveLowerCase: InputValidator = {
    Title: "Character not found",
    Regex: new RegExp(".*[a-z].*"),
    ErrorMessage: "Must have one lowercase character. ",
};

export const mustHaveUpperCase: InputValidator = {
    Title: "Character not found",
    Regex: new RegExp(".*[A-Z].*"),
    ErrorMessage: "Must have one uppercase character. ",
};

export const mustHaveSpecialCharacter: InputValidator = {
    Title: "Character not found",
    Regex: new RegExp("[!@#\\-\\\\^\\$\\.\\|\\?\\*\\+\\(\\)\\[\\]\\{\\}]"),
    ErrorMessage: "Must have one special character. ",
};