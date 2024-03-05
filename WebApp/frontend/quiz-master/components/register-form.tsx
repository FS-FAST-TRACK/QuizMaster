"use client";

import { Button, PasswordInput, TextInput, Tooltip } from "@mantine/core";
import { matchesField, useForm } from "@mantine/form";
import logo from "/public/quiz-master-logo.png";
import Image from "next/image";
import Link from "next/link";
import { notification } from "@/lib/notifications";
import { useDisclosure } from "@mantine/hooks";
import { redirect, useRouter } from "next/navigation";
import { ReactNode, useEffect, useState } from "react";
import { validatorFactory } from "@/lib/validation/creators";
import {
    isRequired,
    mustBeEmail,
    mustHaveDigit,
    mustHaveLowerCase,
    mustHaveSpecialCharacter,
    mustHaveUpperCase,
    userNameValidator,
} from "@/lib/validation/regex";
import { validate } from "@/lib/validation/validate";
import { register } from "@/lib/hooks/auth";
import { useErrorRedirection } from "@/utils/errorRedirection";

const maxChar = validatorFactory(50, "max");
const minChar = validatorFactory(3, "min");
const passWordMinChar = validatorFactory(8, "min");

const RegisterForm = () => {
    const [open, handlers] = useDisclosure(false);
    const [visible, handlePasswordVissibility] = useDisclosure(false);
    const [tooltipLabel, setTooltipLabel] = useState<ReactNode | undefined>();
    const { redirectToError } = useErrorRedirection();
    const router = useRouter();

    const form = useForm({
        initialValues: {
            firstName: "",
            lastName: "",
            userName: "",
            email: "",
            password: "",
            confirmPassword: "",
        },
        validateInputOnBlur: true,
        validate: {
            firstName: (value) => {
                const validators = [isRequired, minChar, maxChar];
                return validate(value, validators);
            },
            lastName: (value) => {
                const validators = [isRequired, minChar, maxChar];
                return validate(value, validators);
            },
            userName: (value) => {
                const validators = [
                    isRequired,
                    minChar,
                    maxChar,
                    userNameValidator,
                ];
                return validate(value, validators);
            },
            email: (value) => {
                const validators = [isRequired, minChar, maxChar, mustBeEmail];
                return validate(value, validators);
            },
            password: (value) => {
                const validators = [
                    isRequired,
                    passWordMinChar,
                    maxChar,
                    mustHaveLowerCase,
                    mustHaveUpperCase,
                    mustHaveDigit,
                    mustHaveSpecialCharacter,
                ];
                return validate(value, validators);
            },
            confirmPassword: matchesField("password", "Passwords do not match"),
        },
    });

    const signUp = async (newUser: {
        firstName: string;
        lastName: string;
        userName: string;
        email: string;
        password: string;
    }) => {
        handlers.open();
        try {
            const response = await register(newUser);

            if (response.statusCode < 300) {
                notification({ type: "success", title: response.message });
                //redirect("/auth/login");
                router.push("/auth/login")
            } else {
                notification({ type: "error", title: response.message });
            }
        } catch (e) {
            notification({
                type: "error",
                title: "Something went wrong",
            });
            redirectToError();
        }
        handlers.close();
    };
    useEffect(() => {
        if (!form.isValid()) {
            setTooltipLabel(<div>Fill up the missing fields.</div>);
        } else {
            setTooltipLabel(<div>Create your account now!</div>);
        }
    }, [form.values]);

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
                        visible={visible}
                        onVisibilityChange={handlePasswordVissibility.toggle}
                    />
                    <PasswordInput
                        radius="6px"
                        placeholder="Confirm password"
                        name="confirmPassword"
                        id="confirmPassword"
                        {...form.getInputProps("confirmPassword")}
                        visible={visible}
                        onVisibilityChange={handlePasswordVissibility.toggle}
                    />
                </div>

                <div className=" flex flex-col justify-center items-center gap-[8px]">
                    <Tooltip label={tooltipLabel}>
                        <Button
                            size="18px"
                            h="52px"
                            fw={700}
                            color="#FF6633"
                            type="submit"
                            fullWidth
                            radius={6}
                            disabled={open}
                            className="bg-[#FF6633]"
                        >
                            {open ? "Creating Account..." : "Create account"}
                        </Button>
                    </Tooltip>
                    <p className="text-sm">
                        Already have an account?{" "}
                        <Link
                            href={"/auth/login"}
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
