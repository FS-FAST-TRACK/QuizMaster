"use client";

import {
    Button,
    Container,
    PasswordInput,
    Text,
    TextInput,
} from "@mantine/core";
import {
    isEmail,
    isNotEmpty,
    matches,
    matchesField,
    useForm,
} from "@mantine/form";
import logo from "/public/quiz-master-logo.png";
import { signIn } from "next-auth/react";
import Image from "next/image";
import Link from "next/link";
const RegisterForm = () => {
    const form = useForm({
        initialValues: {
            firstName: "",
            lastName: "",
            userName: "",
            email: "",
            password: "",
            confirmPassword: "",
        },
        validate: {
            firstName: isNotEmpty("First name is required"),
            lastName: isNotEmpty("Last name is required"),
            userName: isNotEmpty("Username is required"),
            email: isEmail("Invalid Email"),
            password: isNotEmpty("Password is required"),
            confirmPassword: matchesField("password", "Passwords do not match"),
        },
    });
    const login = async (username, password) => {
        const response = await fetch(
            `${process.env.QUIZMASTER_GATEWAY}/auth/login`,
            {
                method: "POST",
                body: JSON.stringify({ username, password }),
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
        const data = await response.json();
        console.log(data);
        await signIn("credentials", {
            jwt: data.token,
        });
    };

    return (
        <div className="bg-white p-[64px] w-[550px] rounded-[16px] flex flex-col gap-[32px]  items-center shadow-[0px_4px_4px_rgba(0,0,0,.25)]">
            <Image
                className=""
                src={logo}
                alt="QuizMaster Logo"
                width={100}
                height={100}
            />
            <p className="font-semibold text-[24px] ">Create your account</p>
            <form
                className="space-y-5 w-full flex-col flex gap-[40px]"
                onSubmit={form.onSubmit((e) => {
                    login(e.username, e.password);
                })}
            >
                <div className="flex flex-col gap-10 w-full">
                    <div className="flex gap-4 items-end">
                        <TextInput
                            radius="6px"
                            placeholder="First name"
                            name="firstName"
                            id="firstName"
                            {...form.getInputProps("firstName")}
                        />
                        <TextInput
                            radius="6px"
                            placeholder="Last name"
                            name="lastName"
                            id="lastName"
                            {...form.getInputProps("lastName")}
                        />
                    </div>

                    <TextInput
                        radius="6px"
                        placeholder="Email"
                        name="email"
                        id="email"
                        {...form.getInputProps("email")}
                    />
                    <PasswordInput
                        radius="6px"
                        placeholder="Password"
                        name="password"
                        id="password"
                        {...form.getInputProps("password")}
                    />
                    <PasswordInput
                        radius="6px"
                        placeholder="Confirm password"
                        name="confirmPassword"
                        id="confirmPassword"
                        {...form.getInputProps("confirmPassword")}
                    />
                </div>

                <div className=" flex flex-col justify-center items-center gap-[8px]">
                    <Button
                        size="18px"
                        h="52px"
                        fw={700}
                        color="#FF6633"
                        type="submit"
                        fullWidth
                        radius={6}
                    >
                        Create account
                    </Button>
                    <p className="text-sm">
                        Already have an account?{" "}
                        <Link
                            href={"/"}
                            className="font-medium hover:underline cursor-pointer"
                        >
                            Login
                        </Link>
                    </p>
                </div>
            </form>
        </div>
    );
};

export default RegisterForm;
