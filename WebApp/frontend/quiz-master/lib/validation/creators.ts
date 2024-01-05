import { InputValidator } from "./regex";

const minimumCharacter = (minLen: number, error?: string) : InputValidator => {
    return {
        ErrorMessage: error? error:  `Must contain at least ${minLen} characters.`,
        Regex: new RegExp(`.{${minLen},}`)
    }
}

const maximumCharater = (maxLen: number, error?: string) : InputValidator => {
    return {
        ErrorMessage: error? error:  `Must contain at least ${maxLen} characters.`,
        Regex: new RegExp(`.{0,${maxLen}}`)
    }
}

const emptyValidator = () : InputValidator=> {
    return {
        ErrorMessage: "",
        Regex: new RegExp(".*")
    }
}

export const validatorFactory = (len: number, type: "min" | "max", error?: string) => {
    switch (type) {
        case "max":
            return maximumCharater(len, error)
        case "min":
            return minimumCharacter(len, error)
            default: 
            return emptyValidator()
    }
}