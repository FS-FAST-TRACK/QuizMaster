"use client";

import { Button, Input, PasswordInput, Text, TextInput } from "@mantine/core";
import { isNotEmpty, useForm } from "@mantine/form";
import logo from "/public/quiz-master-logo.png";
import { signIn } from "next-auth/react";
import Image from "next/image";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { notification } from "@/lib/notifications";
import { useDisclosure } from "@mantine/hooks";
import { login } from "@/lib/hooks/auth";
import { useErrorRedirection } from "@/utils/errorRedirection";

const LoginForm = ({ callbackUrl }: { callbackUrl: string }) => {
    const router = useRouter();
    const { redirectToError } = useErrorRedirection();

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

    const handleLogin = async (username: string, password: string) => {
        handlers.open();
        try {
            const response = await login({ username, password });
            console.log(callbackUrl);
            // Sign In the user if response is with 200
            if (response.type === "success") {
                localStorage.setItem("token", response.data.token); //this is just temporary.
                notification({ type: "success", title: response.message });
                await signIn("credentials", {
                    jwt: response.data.token,
                });
                router.push(callbackUrl);
            } else {
                notification({ type: "error", title: response.message });
            }
        } catch (e) {
            notification({ type: "error", title: "Something went wrong" });
            redirectToError();
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
                    handleLogin(e.username, e.password);
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
