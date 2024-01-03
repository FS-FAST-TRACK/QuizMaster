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
import { notification } from "@/lib/notifications";
import { useDisclosure } from "@mantine/hooks";
import { redirect } from "next/navigation";
import { useCallback } from "react";

const RegisterForm = () => {
    const [open, handlers] = useDisclosure(false);

    const form = useForm({
        initialValues: {
            firstName: "",
            lastName: "",
            userName: "",
            email: "",
            password: "",
            confirmPassword: "",
        },
        validateInputOnChange: true,
        validate: {
            firstName: (value) => {
                if (value.length > 50) {
                    return "Max of 50 characters.";
                }
                return isNotEmpty("First name is required")(value);
            },
            lastName: (value) => {
                if (value.length > 50) {
                    return "Max of 50 characters.";
                }
                return isNotEmpty("Last name is required")(value);
            },
            userName: (value) => {
                if (value.length > 50) {
                    return "Max of 50 characters";
                }
                return isNotEmpty("Username is required")(value);
            },
            email: (value) => {
                if (value.length > 250) {
                    return "Max of 250 characters";
                }
                return isEmail("Invalid Email")(value);
            },
            password: (value) => {
                if (value.length > 100) {
                    return "Max of 100 characters.";
                }
                return isNotEmpty("Password is required")(value);
            },
            confirmPassword: matchesField("password", "Passwords do not match"),
        },
    });

    const handleRedirect = useCallback(() => {
        redirect("/auth/login");
    }, [open]);

    const signUp = async (newUser: {
        firstName: string;
        lastName: string;
        userName: string;
        email: string;
        password: string;
    }) => {
        handlers.open();
        try {
            const response = await fetch(
                `${process.env.QUIZMASTER_GATEWAY}/gateway/api/account/create`,
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

            console.log(data, response.status);
            if (response.status < 300) {
                signIn("credentials", {
                    jwt: data.token,
                });
            } else {
                notification({ type: "error", title: data.message });
            }
        } catch (e) {
            notification({
                type: "error",
                title: "Something went wrong",
            });
        }
        handlers.close();
    };

    return (
        <div className="bg-white p-5 md:p-[64px] w-full max-w-[550px] rounded-[16px] flex flex-col gap-[32px]  items-center shadow-[0px_4px_4px_rgba(0,0,0,.25)]">
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
                    signUp(form.values);
                })}
            >
                <div className="flex flex-col gap-10 w-full">
                    <div className="flex flex-col md:flex-row gap-4 w-full [&>*]:grow">
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
                    <TextInput
                        radius="6px"
                        placeholder="Username"
                        name="userName"
                        id="userName"
                        {...form.getInputProps("userName")}
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
                        disabled={open}
                    >
                        {open ? "Creating Account..." : "Create account"}
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
