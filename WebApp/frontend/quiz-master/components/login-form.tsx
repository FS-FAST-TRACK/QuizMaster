"use client";

import {
    Button,
    Container,
    Input,
    PasswordInput,
    Text,
    TextInput,
} from "@mantine/core";
import { isNotEmpty, useForm } from "@mantine/form";
import logo from "/public/quiz-master-logo.png";
import { signIn } from "next-auth/react";
import Image from "next/image";
import Link from "next/link";
import { redirect } from "next/navigation";
import { notification } from "@/lib/notifications";
import { useDisclosure } from "@mantine/hooks";

const LoginForm = ({ callbackUrl }: { callbackUrl: string }) => {
    const form = useForm({
        initialValues: {
            username: "",
            password: "",
        },
        validateInputOnBlur: true,
        validate: {
            username: isNotEmpty("Username is required"),
            password: isNotEmpty("Password is required"),
        },
    });

    const [open, handlers] = useDisclosure(false);

    const login = async (username: string, password: string) => {
        handlers.open();
        try {
            const response = await fetch(
                `${process.env.QUIZMASTER_GATEWAY}/gateway/api/auth/login`,
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
            // Sign In the user if response is with 200
            if (response.status < 300) {
                await signIn("credentials", {
                    jwt: data.token,
                });
                redirect(callbackUrl);
            } else {
                notification({ type: "error", title: data.message });
            }
        } catch (e) {
            console.log(e);
        }
        handlers.close();
    };

    return (
        <div className="bg-white p-[64px] w-[450px] rounded-[16px] flex flex-col gap-[32px]  items-center shadow-[0px_4px_4px_rgba(0,0,0,.25)]">
            <Image
                className=""
                src={logo}
                alt="QuizMaster Logo"
                width={100}
                height={100}
            />
            <p className="font-semibold text-[24px] ">Login to your account</p>
            <form
                className="space-y-5 w-full flex-col flex gap-[40px]"
                onSubmit={form.onSubmit((e) => {
                    login(e.username, e.password);
                })}
            >
                <div className="flex flex-col gap-[15px]">
                    <Input.Wrapper className="space-y-4">
                        <Input.Label size="16px" fw="500" htmlFor="username">
                            Username
                        </Input.Label>

                        <TextInput
                            radius="6px"
                            placeholder="Username"
                            name="username"
                            height="42"
                            id="username"
                            {...form.getInputProps("username")}
                        />
                    </Input.Wrapper>
                    <Text size="16px" fw="500">
                        Password
                    </Text>
                    <PasswordInput
                        radius="6px"
                        placeholder="Password"
                        name="password"
                        {...form.getInputProps("password")}
                    />
                    <p className="text-sm self-end w-full flex justify-end ">
                        Forgot password?
                    </p>
                </div>
                <div className=" flex flex-col justify-center items-center gap-[8px]">
                    <Button
                        size="18px"
                        h="52px"
                        radius={6}
                        fw={700}
                        color="#FF6633"
                        type="submit"
                        fullWidth
                        disabled={open}
                    >
                        {open ? "Logging in..." : "Login"}
                    </Button>
                    <p className="text-sm">
                        Don't have an account yet?{" "}
                        <Link
                            href={"/auth/signup"}
                            className="font-medium hover:underline cursor-pointer"
                        >
                            Sign up
                        </Link>
                    </p>
                </div>
            </form>
        </div>
    );
};

export default LoginForm;
