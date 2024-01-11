"use client";

import { Modal, Pagination, Select, Table, TextInput } from "@mantine/core";
import { useEffect, useRef, useState } from "react";
import { useDownloadExcel } from "react-export-table-to-excel";
import { useDisclosure } from "@mantine/hooks";
import { UserAudit, UserAuditTrail } from "@/lib/definitions";
import { fetchUserAudit } from "@/lib/hooks/accountAudit";

export default function Page() {
    const tableRef = useRef(null);

    const currentDate = new Date();
    const currentDateAsString = currentDate.toLocaleDateString("en-US", {
        month: "long",
        day: "numeric",
        year: "numeric",
    });

    const [formattedFirstDate, setFormattedFirstDate] = useState(
        `${new Date().getFullYear()}-${String(
            new Date().getMonth() + 1
        ).padStart(2, "0")}-01`
    );

    const [formattedCurrentDate, setFormattedCurrentDate] = useState(
        `${currentDate.getFullYear()}-${String(
            currentDate.getMonth() + 1
        ).padStart(2, "0")}-${String(currentDate.getDate()).padStart(2, "0")}`
    );

    const { onDownload } = useDownloadExcel({
        currentTableRef: tableRef.current,
        filename: `Account Audit - ${currentDateAsString}`,
        sheet: "Users",
    });

    const startDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        1
    );

    const [searchedText, setSearchedText] = useState("");

    const propertyHeadersToSearch = [
        "userAuditTrailId",
        "action",
        "details",
        "userRole",
    ];

    const [userAudit, setUserAudit] = useState<UserAudit[]>([]);
    const [filteredData, setFilteredData] = useState<UserAudit[]>([]);
    const [userType, setUserType] = useState<string | null>("All");

    useEffect(() => {
        const fetchData = async () => {
            try {
                const { data } = await fetchUserAudit();
                setUserAudit(data);
                setFilteredData(data);
            } catch (err) {
                console.log(err);
            }
        };
        fetchData();
    }, []);

    const filteredDataByDateAndSearch = () => {
        const filtered = userAudit.filter((entry) => {
            const entryTimestamp = new Date(entry.timestamp);
            const startOfDayFirstDate = new Date(formattedFirstDate);
            const endOfDayCurrentDate = new Date(formattedCurrentDate);
            endOfDayCurrentDate.setHours(23, 59, 59, 999);

            const isDateInRange =
                entryTimestamp >= startOfDayFirstDate &&
                entryTimestamp <= endOfDayCurrentDate;

            const isTextMatched = propertyHeadersToSearch.some((header) => {
                const entryValue = entry[header];
                const isMatched =
                    entryValue !== undefined &&
                    entryValue
                        .toString()
                        .toLowerCase()
                        .includes(searchedText.toLowerCase());

                return isMatched;
            });

            if (userType == "All") {
                return isDateInRange && isTextMatched;
            } else {
                const filterByUserType =
                    entry.userRole.toLowerCase() == userType?.toLowerCase();

                return isDateInRange && isTextMatched && filterByUserType;
            }
        });

        setFilteredData(filtered);
    };

    useEffect(() => {
        filteredDataByDateAndSearch();
    }, [formattedFirstDate, formattedCurrentDate, searchedText, userType]);

    return (
        <div className="w-full h-full flex justify-center">
            <div className="w-[90%]  h-full py-5">
                <div className="w-full  flex items-center justify-between gap-5 className ">
                    <div className="w-[60%] flex flex-col gap-1 p-1.5 ">
                        <h1 className="text-sm font-medium">Search Logs</h1>
                        <input
                            type="text"
                            placeholder="Search activity logs"
                            onChange={(e) => setSearchedText(e.target.value)}
                            className="p-2 px-2 rounded-sm border border-gray-200 shadow-sm text-sm font-light"
                        />
                    </div>
                    <div className="flex justify-end className">
                        <div className="flex gap-3 flex-end className  w-[90%]">
                            <Select
                                label="Action"
                                size="sm"
                                placeholder="Pick value"
                                data={["React", "Angular", "Vue", "Svelte"]}
                            />
                            <Select
                                label="Type"
                                placeholder="Pick value"
                                data={["React", "Angular", "Vue", "Svelte"]}
                            />
                            <Select
                                label="Users"
                                placeholder="Pick value"
                                value={userType}
                                onChange={(value) => setUserType(value)}
                                data={["All", "User", "Admin"]}
                            />
                        </div>
                    </div>
                </div>

                <div className=" mt-5 w-full">
                    <div>
                        <h1 className="text-sm font-medium">Date Range</h1>
                        <div className="w-full  flex items-center justify-between className">
                            <div className="flex gap-3 p-1.5">
                                <div className="relative">
                                    <input
                                        name="start"
                                        type="date"
                                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-sm focus:ring-blue-500 focus:border-blue-500 block w-full p-1.5"
                                        onChange={(e) =>
                                            setFormattedFirstDate(
                                                e.target.value
                                            )
                                        }
                                        value={formattedFirstDate}
                                    />
                                </div>
                                <h1 className="font-bold text-xl">-</h1>

                                <div className="relative">
                                    <input
                                        name="start"
                                        type="date"
                                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-sm focus:ring-blue-500 focus:border-blue-500 block w-full p-1.5"
                                        onChange={(e) =>
                                            setFormattedCurrentDate(
                                                e.target.value
                                            )
                                        }
                                        value={formattedCurrentDate}
                                    />
                                </div>
                            </div>

                            <button
                                className="flex gap-5 border border-gray-300 rounded-sm px-4 py-1.5 text-sm items-center bg-white shadow-sm"
                                onClick={onDownload}
                            >
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    height="16"
                                    width="16"
                                    viewBox="0 0 512 512"
                                >
                                    <path d="M288 109.3V352c0 17.7-14.3 32-32 32s-32-14.3-32-32V109.3l-73.4 73.4c-12.5 12.5-32.8 12.5-45.3 0s-12.5-32.8 0-45.3l128-128c12.5-12.5 32.8-12.5 45.3 0l128 128c12.5 12.5 12.5 32.8 0 45.3s-32.8 12.5-45.3 0L288 109.3zM64 352H192c0 35.3 28.7 64 64 64s64-28.7 64-64H448c35.3 0 64 28.7 64 64v32c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V416c0-35.3 28.7-64 64-64zM432 456a24 24 0 1 0 0-48 24 24 0 1 0 0 48z" />
                                </svg>
                                Export logs
                            </button>
                        </div>
                    </div>
                </div>

                <div className="relative overflow-x-auto mt-5 rounded-lg border border-gray-200 h-[70%] bg-white ">
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
                            {filteredData.map((user: UserAudit) => (
                                <tr className="bg-white border-b ">
                                    <td className="px-6 py-4 font-medium">
                                        {user.userAuditTrailId}
                                    </td>
                                    <td className="px-6 py-4 font-medium">
                                        {user.userRole}
                                    </td>
                                    <td className="px-6 py-4">
                                        {user.timestamp.toString()}
                                    </td>
                                    <td className="px-6 py-4">{user.action}</td>
                                    <td className="px-6 py-4">{user.action}</td>
                                    <td className="px-6 py-4">
                                        {user.details}
                                    </td>
                                    <td className="px-3 py-4">
                                        <button>
                                            <svg
                                                xmlns="http://www.w3.org/2000/svg"
                                                height="16"
                                                width="14"
                                                viewBox="0 0 448 512"
                                            >
                                                <path d="M8 256a56 56 0 1 1 112 0A56 56 0 1 1 8 256zm160 0a56 56 0 1 1 112 0 56 56 0 1 1 -112 0zm216-56a56 56 0 1 1 0 112 56 56 0 1 1 0-112z" />
                                            </svg>
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}
