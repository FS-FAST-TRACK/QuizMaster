import { InputValidator } from "./regex";

export const validate = (value: string, validators: InputValidator[]) : string | null => {
    const errorMessage =  validators.map((checker) => {
        return !checker.Regex.test(value)
            ? checker.ErrorMessage
            : "";
    })
    .reduce((prev, curr) => prev + curr, "")
    return errorMessage !== "" ? errorMessage : null;
}
