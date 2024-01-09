"use client";
import { Dispatch, SetStateAction, useState } from "react";
import SaveCancelButton from "./SaveCancelButton";
export default function EditFieldWithButton({
    title,
    value,
    onInput,
    changeBtnTitle,
    inputType,
    onSave,
    onCancel,
}: {
    title: string;
    value: string;
    onInput?: Dispatch<SetStateAction<string>>;
    changeBtnTitle?: string;
    inputType?: string;
    onSave?: (s: Array<string>) => void;
    onCancel?: () => void;
}) {
    const [editting, setEditting] = useState<boolean>(false);
    const [currentPassword, setCurrentPassword] = useState<string>("");
    const [newPassword, setNewPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");
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
                                onSave={() => {
                                    if (inputType === "password") {
                                        if (currentPassword === "") {
                                            alert("Current password is empty");
                                            return;
                                        }
                                        if (currentPassword === newPassword) {
                                            alert(
                                                "Old password cannot be the same as new password"
                                            );
                                            return;
                                        }
                                        if (newPassword !== confirmPassword) {
                                            alert(
                                                "New Password and Confirm Password must match"
                                            );
                                            return;
                                        }
                                        onSave &&
                                            onSave([
                                                currentPassword,
                                                newPassword,
                                            ]);
                                        setEditting(false);
                                    } else {
                                        onSave && onSave([value]);
                                        setEditting(false);
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
