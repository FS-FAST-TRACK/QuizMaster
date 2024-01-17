import { PaginationMetadata, QuestionDifficulty } from "@/lib/definitions";

export interface CustomResponse  {
    statusCode: number;
    type: "success" | "error" | "fail";
    data: any;
    message: string;
};

export interface LoginResponse extends CustomResponse  {
    data: {
        token: string
    }
}

export interface RegisterResponse extends CustomResponse  {
    data: null,
}
