"use client";
import EditField from "@/components/Commons/profile/EditField";
import EditFieldWithButton from "@/components/Commons/profile/EditFieldWithButton";
import SaveCancelButton from "@/components/Commons/profile/SaveCancelButton";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";
import { useState } from "react";

export default function Page() {
    const [firstName, setFirstName] = useState<string>("Jay");
    const [lastName, setLastName] = useState<string>("Abejar");
    const [userName, setUserName] = useState<string>("jaymar921");
    const [email, setEmail] = useState<string>("jay@gmail.com");
    const [editToggled, setEditToggled] = useState<boolean>(false);
    return (
        <>
            <div>
                <div>
                    <h1 className="text-[32px] font-bold text-[#3C3C3C]">
                        Profile Details
                    </h1>
                </div>
                <div className="pt-5 font-bold flex items-center pb-2">
                    <p className="text-[#3C3C3C]">User Details</p>
                    {!editToggled && (
                        <button
                            onClick={() => setEditToggled(!editToggled)}
                            className="m-1 p-1 px-2 border border-black-1 rounded text-[12px] font-normal"
                        >
                            Edit
                        </button>
                    )}
                </div>
                <hr />
                <div className="grid grid-cols-2 gap-2 w-[80%] py-2">
                    <EditField
                        editting={editToggled}
                        title={"First Name"}
                        value={firstName}
                        onInput={setFirstName}
                    />
                    <EditField
                        editting={editToggled}
                        title={"Last Name"}
                        value={lastName}
                        onInput={setLastName}
                    />
                    <EditField
                        editting={editToggled}
                        title={"Username"}
                        value={userName}
                        onInput={setUserName}
                    />
                    <EditField
                        editting={false}
                        title={"Roles"}
                        value={"Admin"}
                        onInput={() => {}}
                    />
                    {editToggled && (
                        <SaveCancelButton
                            onSave={() => {}}
                            onCancel={() => {
                                setEditToggled(false);
                            }}
                        />
                    )}
                </div>
                <div className="pt-5 font-bold items-center pb-2 text-[#3C3C3C]">
                    <p>Account Details</p>
                </div>
                <hr />
                <div className="py-2">
                    <EditFieldWithButton
                        title="Email"
                        value={email}
                        onInput={setEmail}
                        changeBtnTitle="Change"
                    />
                    <EditFieldWithButton
                        title="Password"
                        value=""
                        inputType="password"
                        changeBtnTitle="Change Password"
                    />
                </div>
                <div className="pt-5 font-bold flex items-center pb-2 text-[#3C3C3C]">
                    <p>Delete Account</p>
                </div>
                <hr />
                <div className="flex border-2 border-red-500 text-red-500 w-fit p-2 text-[15px] rounded-md mt-4 text-center">
                    <ExclamationTriangleIcon className="w-[25px] mr-2" />
                    <p className="">
                        Warning: Deleting an account will be permanent and
                        cannot be undone.
                    </p>
                </div>
                <button className="bg-red-500 text-white rounded text-[15px] p-2 mt-4">
                    Delete my account
                </button>
            </div>
        </>
    );
}
