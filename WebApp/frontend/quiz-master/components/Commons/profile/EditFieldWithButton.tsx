"use client";
import { Dispatch, SetStateAction, useState } from "react";
import SaveCancelButton from "./SaveCancelButton";
import { validatorFactory } from "@/lib/validation/creators";
import {
    isRequired,
    mustBeEmail,
    mustHaveDigit,
    mustHaveLowerCase,
    mustHaveSpecialCharacter,
    mustHaveUpperCase,
} from "@/lib/validation/regex";
import { validate } from "@/lib/validation/validate";

export default function EditFieldWithButton({
    title,
    value,
    onInput,
    changeBtnTitle,
    inputType,
    onSave,
    onCancel,
    onError,
}: {
    title: string;
    value: string;
    onInput?: Dispatch<SetStateAction<string>>;
    changeBtnTitle?: string;
    inputType?: string;
    onSave?: (s: Array<string>) => void | Promise<boolean>;
    onCancel?: () => void;
    onError?: (message: string) => void;
}) {
    const [editting, setEditting] = useState<boolean>(false);
    const [currentPassword, setCurrentPassword] = useState<string>("");
    const [newPassword, setNewPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");
    const [confirmEnabled, setConfirmDisabled] = useState<boolean>(true);

    return (
        <div className="relative w-[100%]">
            <p className={`${editting && "font-bold"}`}>{title}</p>
            <div className="flex">
                {!editting && value !== "" && (
                    <p className={`text-[18px] font-bold py-2`}>{value}</p>
                )}
                {editting && (
                    <div className="block">
                        <form
                            onSubmit={(e) => {
                                e.preventDefault();
                            }}
                        >
                            <input
                                name={`${
                                    inputType === "password"
                                        ? "current-password"
                                        : Math.floor(Math.random() * 999999)
                                }`}
                                className="border border-black-200 rounded outline-1 p-2"
                                autoComplete={`${
                                    inputType === "password"
                                        ? "current-password"
                                        : ""
                                }`}
                                placeholder={`${
                                    inputType === "password"
                                        ? "Current Password"
                                        : ""
                                }`}
                                type={inputType ? inputType : "text"}
                                onChange={(e) => {
                                    onInput && onInput(e.target.value);
                                    if (inputType === "password") {
                                        setCurrentPassword(e.target.value);
                                    }
                                }}
                                value={`${
                                    inputType !== "password"
                                        ? value
                                        : currentPassword
                                }`}
                            />
                        </form>
                        <form
                            onSubmit={(e) => {
                                e.preventDefault();
                            }}
                        >
                            {inputType === "password" && (
                                <input
                                    placeholder={`${
                                        inputType === "password"
                                            ? "New Password"
                                            : ""
                                    }`}
                                    name="new-password"
                                    autoComplete="new-password"
                                    className="border border-black-200 rounded outline-1 p-2 block"
                                    type={inputType ? inputType : "text"}
                                    onChange={(e) =>
                                        setNewPassword(e.target.value)
                                    }
                                    value={`${newPassword}`}
                                />
                            )}
                            {inputType === "password" && (
                                <input
                                    placeholder={`${
                                        inputType === "password"
                                            ? "Confirm Password"
                                            : ""
                                    }`}
                                    name="confirm-password"
                                    autoComplete="confirm-password"
                                    className="border border-black-200 rounded outline-1 p-2 block"
                                    type={inputType ? inputType : "text"}
                                    onChange={(e) =>
                                        setConfirmPassword(e.target.value)
                                    }
                                    value={`${confirmPassword}`}
                                />
                            )}
                        </form>
                    </div>
                )}
                {!editting && (
                    <button
                        className={`p-2 ${
                            value !== "" && "ml-2"
                        } border border-black rounded-md text-black text-[12px]`}
                        onClick={() => setEditting(true)}
                    >
                        {changeBtnTitle}
                    </button>
                )}
                {editting && (
                    <>
                        <div className="block ml-2 ">
                            <SaveCancelButton
                                className="h-[32px] items-center"
                                enabled={confirmEnabled}
                                onSave={() => {
                                    if (inputType === "password") {
                                        const maxChar = validatorFactory(
                                            50,
                                            "max"
                                        );
                                        const passWordMinChar =
                                            validatorFactory(8, "min");
                                        const validators = [
                                            isRequired,
                                            passWordMinChar,
                                            maxChar,
                                            mustHaveLowerCase,
                                            mustHaveUpperCase,
                                            mustHaveDigit,
                                            mustHaveSpecialCharacter,
                                        ];
                                        // check empty
                                        if (!currentPassword) {
                                            onError &&
                                                onError(
                                                    "Current password is empty"
                                                );
                                            return;
                                        }
                                        // check if old password is same as new
                                        if (currentPassword === newPassword) {
                                            onError &&
                                                onError(
                                                    "Old password cannot be the same as new password"
                                                );
                                            return;
                                        }
                                        const validatePassword = validate(
                                            newPassword,
                                            validators
                                        );
                                        if (null != validatePassword) {
                                            onError &&
                                                onError(validatePassword);
                                            return;
                                        }
                                        // check if new and confirm is matched
                                        if (newPassword !== confirmPassword) {
                                            onError &&
                                                onError(
                                                    "New Password and Confirm Password must match"
                                                );
                                            return;
                                        }
                                        if (onSave != null) {
                                            setConfirmDisabled(false);
                                            Promise.resolve(
                                                onSave([
                                                    currentPassword,
                                                    newPassword,
                                                ])
                                            ).then((res) => {
                                                if (res) setEditting(false);
                                                setConfirmDisabled(true);
                                            });
                                        }
                                    } else {
                                        if (onSave != null) {
                                            setConfirmDisabled(false);
                                            Promise.resolve(
                                                onSave([value])
                                            ).then((res) => {
                                                if (res) setEditting(false);
                                                setConfirmDisabled(true);
                                            });
                                        }
                                    }
                                }}
                                onCancel={() => {
                                    setEditting(false);
                                    setNewPassword("");
                                    setConfirmPassword("");
                                    setCurrentPassword("");
                                    onCancel && onCancel();
                                }}
                            />
                            {inputType === "password" && (
                                <p className="mt-2 text-[10px] text-left w-[100%]">
                                    After clicking the save changes, a
                                    confirmation email will be sent to you
                                </p>
                            )}
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}
