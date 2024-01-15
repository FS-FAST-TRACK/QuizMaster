"use client";

import { Modal, Pagination, Select, Table, TextInput } from "@mantine/core";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useDownloadExcel } from "react-export-table-to-excel";
import { useDisclosure } from "@mantine/hooks";
import { AuditTrail, UserAudit, UserAuditTrail } from "@/lib/definitions";

import AuditTable from "@/components/Commons/AuditTable/AuditTable";
import { fetchAudit } from "@/lib/hooks/audit";
import {
    actionValues,
    propertyHeadersToSearch,
    selectTypeValues,
    setUriAudit,
} from "./accountDefinitions";

export default function Page() {
    const tableRef = useRef(null);
    const currentDate = new Date();

    const [actionTypeValue, setActionTypeValue] = useState<string[]>();
    const [searchedText, setSearchedText] = useState("");
    const [auditType, setAuditType] = useState<string | null>("Account");
    const [userAudit, setUserAudit] = useState<AuditTrail[]>([]);
    const [filteredData, setFilteredData] = useState<AuditTrail[]>([]);
    const [userType, setUserType] = useState<string | null>("All");
    const [actionType, setActionType] = useState<string | null>("All");

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
        filename: `Account Audit - ${formattedFirstDate} - ${formattedCurrentDate}`,
        sheet: "Users",
    });

    useEffect(() => {
        setUserType("All");
        setActionType("All");
        setActionTypeValue(actionValues(auditType));
        const fetchData = async () => {
            try {
                const { data } = await fetchAudit(setUriAudit(auditType));
                console.log(data);
                setUserAudit(data);
                setFilteredData(data);
            } catch (err) {
                console.log(err);
            }
        };
        fetchData();
    }, [auditType]);

    const filterByUserType = useCallback(
        (entry: AuditTrail) => {
            return (
                userType === "All" ||
                entry.userRole.toLowerCase() === userType?.toLowerCase()
            );
        },
        [userType]
    );

    const filterByActionType = useCallback(
        (entry: AuditTrail) => {
            return (
                actionType === "All" ||
                entry.action.toLowerCase() === actionType?.toLowerCase()
            );
        },
        [actionType]
    );

    const filteredDataByDateAndSearch = useMemo(() => {
        return userAudit.filter((entry: any) => {
            const entryTimestamp = new Date(entry.timestamp);
            const startOfDayFirstDate = new Date(formattedFirstDate);
            const endOfDayCurrentDate = new Date(formattedCurrentDate);
            endOfDayCurrentDate.setHours(23, 59, 59, 999);

            const isDateInRange =
                entryTimestamp >= startOfDayFirstDate &&
                entryTimestamp <= endOfDayCurrentDate;

            const userTypeFilter = filterByUserType(entry);
            const actionTypeFilter = filterByActionType(entry);

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

            return (
                isDateInRange &&
                isTextMatched &&
                userTypeFilter &&
                actionTypeFilter
            );
        });
    }, [
        userAudit,
        formattedFirstDate,
        formattedCurrentDate,
        searchedText,
        filterByUserType,
        filterByActionType,
    ]);

    useEffect(() => {
        setFilteredData(filteredDataByDateAndSearch);
    }, [filteredDataByDateAndSearch]);

    return (
        <div className="w-full h-full flex justify-center">
            <div className="w-[90%]  h-full py-5 overflow-x-auto ">
                <div className="w-full flex flex-col md:flex-row items-center justify-between gap-2">
                    <div className="w-full md:w-[50%] flex flex-col gap-2  ">
                        <h1 className="text-sm font-medium">Search Logs</h1>
                        <input
                            type="text"
                            placeholder="Search activity logs"
                            onChange={(e) => setSearchedText(e.target.value)}
                            className="p-2 px-2 rounded-sm border border-gray-200 shadow-sm text-sm font-light"
                        />
                    </div>
                    <div className="w-full md:w-[40%] flex justify-start md:justify-end ">
                        <div className="flex gap-3 md:flex-end w-full md:w-[90%]">
                            <Select
                                label="Action"
                                size="sm"
                                className="text-sm"
                                onChange={setActionType}
                                value={actionType}
                                defaultValue={"All"}
                                placeholder="Pick value"
                                data={actionTypeValue}
                            />

                            <Select
                                label="Type"
                                placeholder="Pick value"
                                defaultValue={auditType}
                                value={auditType}
                                onChange={(value) => {
                                    setAuditType(value);
                                }}
                                data={selectTypeValues}
                            />
                            <Select
                                label="Users"
                                placeholder="Pick value"
                                defaultValue={userType}
                                value={userType}
                                onChange={setUserType}
                                data={["All", "User", "Admin"]}
                            />
                        </div>
                    </div>
                </div>

                <div className=" mt-5 w-full">
                    <div>
                        <h1 className="text-sm font-medium">Date Range</h1>
                        <div className="w-full  flex items-center justify-between className flex-wrap gap-y-2">
                            <div className="flex gap-3 ">
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

                <AuditTable
                    tableRef={tableRef}
                    data={filteredData}
                    auditType={auditType}
                />
            </div>
        </div>
    );
}
