import { QUIZMASTER_ACCOUNT_POST, QUIZMASTER_AUTH_POST_LOGIN } from "@/api/api-routes";
import { LoginResponse, RegisterResponse } from "@/api/definitions";

export async function login({username, password}: {username: string, password: string})  {
    try {
        const response = await fetch(`${QUIZMASTER_AUTH_POST_LOGIN}`, {
            method: "POST",
            body: JSON.stringify({ username, password }),
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            },
        });
        
        const data = await response.json();
        const isInvalidCredentials = response.status === 401;
        var res : LoginResponse = 
        {
            statusCode: response.status,
            message: isInvalidCredentials? data.message: "Login successfull",
            data: {
                token: data.token
            },
            type: isInvalidCredentials? "fail" : "success"
        };
        
        return res; 
        
    } catch (error) {
        console.error("Connection Error:", error);
        throw new Error("Failed to login user");
    }
}


export async function register(newUser: {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
})  {
    try {
        const response = await fetch(
            `${QUIZMASTER_ACCOUNT_POST}`,
            {
                method: "POST",
                body: JSON.stringify(newUser),
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
        
        const data = await response.json();
        const isRequestValid = !(response.status === 400 || response.status === 409);

        var res : RegisterResponse = 
        {
            statusCode: response.status,
            message: isRequestValid? "Register successfull" : data.message,
            data: null,
            type: isRequestValid ? "success" : "fail"
        };

        if(response.status === 400 || response.status === 409){
            res.message = data.message;
        }

        return res; 
        
    } catch (error) {
        console.error("Connection Error:", error);
        throw new Error("Failed to create user");
    }
}