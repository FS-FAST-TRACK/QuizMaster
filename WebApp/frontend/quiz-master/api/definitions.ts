import { inherits } from "util";

export interface Response  {
    statusCode: number;
    type: "success" | "error" | "fail";
    data: any;
    message: string;
};

export interface LoginResponse extends Response  {
    data: {
        token: string
    }
}

export interface RegisterResponse extends Response  {
    data: null,
}
