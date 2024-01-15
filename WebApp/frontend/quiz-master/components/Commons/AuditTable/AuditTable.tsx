import { AuditTableProps, AuditTrail } from "@/lib/definitions";
import { TrueORFalseData } from "@/lib/questionTypeData";
import { Modal } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useEffect, useState } from "react";

const AuditTable: React.FC<AuditTableProps> = ({
    data,
    tableRef,
    auditType,
}) => {
    const [showOptions, setShowOptions] = useState(false);
    const [opened, { open, close }] = useDisclosure(false);
    const [auditTrail, setAuditTrail] = useState<AuditTrail>();

    useEffect(() => {
        setShowOptions(false);
    }, [opened]);

    const formatNewValues = (newValuesString: any) => {
        try {
            const newValues = JSON.parse(newValuesString);
            return Object.keys(newValues).map(
                (key) => `${key}: ${newValues[key]}`
            );
        } catch (error) {
            console.error("Error parsing newValues:", error);
            return "Invalid New Values";
        }
    };

    return (
        <div className="relative overflow-x-auto mt-5 rounded-lg border border-gray-200 h-[70%] bg-white ">
            <Modal
                opened={opened}
                onClose={close}
                title={auditType}
                centered
                size={"lg"}
            >
                <h1 className="font-semibold ">
                    {auditTrail?.details} by: {auditTrail?.userName}
                </h1>

                <div className="grid grid-cols-2 gap-4 mt-5">
                    <div className="flex flex-col gap-1">
                        <p className="text-sm text-gray-500">Audit Type</p>
                        <p className="font-semibold text">{auditType}</p>
                    </div>

                    <div className="flex flex-col gap-1">
                        <p className="text-sm text-gray-500">Action</p>
                        <p className="font-semibold text">
                            {auditTrail?.action}
                        </p>
                    </div>

                    <div className="flex flex-col gap-1">
                        <p className="text-sm text-gray-500">
                            Action taken by:
                        </p>
                        <p className="font-semibold text flex items-center gap-2">
                            {auditTrail?.userName}
                            <span className="text-gray-500 text-sm">
                                {`(${auditTrail?.userRole})`}
                            </span>
                        </p>
                    </div>
                    <div className="flex flex-col gap-1">
                        <p className="text-sm text-gray-500">Timestamp</p>
                        <p className="font-semibold ">
                            {auditTrail?.timestamp
                                .toLocaleString("en-US", {
                                    year: "numeric",
                                    month: "2-digit",
                                    day: "2-digit",
                                    hour: "2-digit",
                                    minute: "2-digit",
                                    second: "2-digit",
                                })
                                .replace("T", " ")}
                        </p>
                    </div>
                </div>

                <div className="h-[20vh]  rounded-md border border-gray-200 w-full mt-10">
                    <h1 className="border-b border-gray-200 text-gray-500 text-sm p-3">
                        Old Values
                    </h1>

                    <p className="p-3 text-gray-700 text-xs">
                        {auditTrail?.oldValues == ""
                            ? "No Old Values"
                            : auditTrail?.oldValues}
                    </p>
                </div>

                <div className="h-[20vh]  rounded-md border border-gray-200 w-full mt-10 flex flex-col gap-2">
                    <h1 className="border-b border-gray-200 text-gray-500 text-sm p-3">
                        New Values
                    </h1>
                    <p className="p-3 text-gray-700 text-xs flex overflow-hidden">
                        {auditTrail?.newValues === ""
                            ? "No New Values"
                            : auditTrail?.newValues}
                    </p>
                </div>
            </Modal>

            <table
                className="w-full text-sm text-left rtl:text-right "
                ref={tableRef}
            >
                <thead className="text-xs text-gray-700  border border-gray-200  ">
                    <tr>
                        <th scope="col" className="px-6 py-3">
                            Trail ID
                        </th>
                        <th scope="col" className="px-6 py-3">
                            User
                        </th>
                        <th scope="col" className="px-6 py-3">
                            Timestamp
                        </th>
                        <th scope="col" className="px-6 py-3">
                            Type
                        </th>
                        <th scope="col" className="px-6 py-3">
                            Action
                        </th>
                        <th scope="col" className="px-6 py-3">
                            Details
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((user: AuditTrail, index) => (
                        <tr className="bg-white border-b text-sm " key={index}>
                            <td className="px-6 py-4 font-medium">
                                {user.userAuditTrailId}
                            </td>
                            <td className="px-6 font-medium ">
                                <p className="font-semibold text-xs">
                                    {user.userName}
                                </p>
                                <p className="text-xs font-light mt-2">
                                    {user.userRole}
                                </p>
                            </td>
                            <td className="px-6 py-4">
                                {user.timestamp
                                    .toLocaleString("en-US", {
                                        year: "numeric",
                                        month: "2-digit",
                                        day: "2-digit",
                                        hour: "2-digit",
                                        minute: "2-digit",
                                        second: "2-digit",
                                    })
                                    .replace("T", " ")}
                            </td>

                            <td className="px-6 py-4">{auditType}</td>
                            <td className="px-6 py-4">{user.action}</td>
                            <td className="px-6 py-4">{user.details}</td>
                            <td className="py-4 px-6">
                                <div className=" className  w-[150px] p-4 relative">
                                    <button
                                        className="className w-full"
                                        onClick={() => {
                                            setAuditTrail(user);
                                            setShowOptions(!showOptions);
                                        }}
                                    >
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            height="16"
                                            width="14"
                                            viewBox="0 0 448 512"
                                        >
                                            <path d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z" />
                                        </svg>
                                    </button>
                                    {showOptions ? (
                                        <button
                                            className="absolute bottom-0 left-2 py-1 text-xs border mt-2 border-gray-200 shadow-md px-3 rounded-sm"
                                            onClick={open}
                                        >
                                            More Details
                                        </button>
                                    ) : (
                                        ""
                                    )}
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default AuditTable;
