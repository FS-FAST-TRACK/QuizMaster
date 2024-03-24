"use client";

import QuizRoomTable from "@/components/Commons/tables/QuizRoomTable";
import { QuizRoom } from "@/lib/definitions/quizRoom";
import { notification } from "@/lib/notifications";
import { QuizRoomPageData } from "@/lib/pagesData";
import { useConnectionStore } from "@/store/ConnectionStore";
import { useQuizRoomsStore } from "@/store/QuizRoomStore";
import { PlusIcon } from "@heroicons/react/24/outline";

import { Anchor, Breadcrumbs } from "@mantine/core";
import Pagination from "@/components/Commons/Pagination";
import { useDebouncedValue, useDisclosure } from "@mantine/hooks";
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";
import { useForm } from "@mantine/form";
import { ResourceParameter } from "@/lib/definitions";
import SearchField from "@/components/Commons/SearchField";


const items = [
    { label: "All", href: "#" },
    { label: "", href: "#" },
].map((item, index) => (
    <Anchor href={item.href} key={index}>
        <p className="text-black">{item.label}</p>
    </Anchor>
));

export default function Page() {
    const [visible, { close, open }] = useDisclosure(true);

    const [searchQuery2, setSearchQuery] = useState<string>("");
    const [debouncedSearchQuery] = useDebouncedValue(searchQuery2, 200);

    const { connection, init } = useConnectionStore();
    const {
        quizRooms,
        pageNumber,
        pageSize,
        searchQuery,
        setQuizRooms,
        getTotalPages,
        setPagination,
    } = useQuizRoomsStore();
    const [totalPages, setTotalPages] = useState<number>();

    const form = useForm<ResourceParameter>({
        initialValues: {
            pageSize: "50",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    const startConnection = useCallback(async () => {
        if (connection && connection.state !== "Connected") {
            await connection.start();
            const token = localStorage.getItem("token");
            connection.invoke("Login", token);
            connection.on("notif", (data) => {
                notification({ type: "success", title: data });
            });
            connection.on("auth_data", (data) => {

                notification({ type: "success", title: data });
            });
            connection.on("QuizRooms", (data: QuizRoom[]) => {
                data.forEach((data) => {
                    data.dateCreated = new Date(data.dateCreated);
                    data.dateUpdated = data.dateUpdated
                        ? new Date(data.dateUpdated)
                        : undefined;
                });
                setQuizRooms(data);
            });
            getAllRooms();
        }
    }, [connection]);

    const getAllRooms = useCallback(async () => {
        if (connection) {
            await connection.invoke("GetAllRooms");
        }
    }, [connection]);

    useEffect(() => {
        if (!connection) {
            init();
        }

        try {
            startConnection();
        } catch (error) {
            notification({
                type: "error",
                title: "Unable to communicate to the hub",
            });
        }
    }, [connection]);

    useEffect(() => {
        setTotalPages(getTotalPages());
    }, [quizRooms, pageNumber, pageSize, searchQuery]);

    useEffect(() => {
        setPagination({
            pageNumber: form.values.pageNumber,
            pageSize: parseInt(form.values.pageSize),
            searchQuery: debouncedSearchQuery,
        });
    }, [form.values, debouncedSearchQuery]);

    return (
        <div className="flex flex-col px-6 md:px-16 md:pb-20 py-5 space-y-5 grow" >
            <Breadcrumbs>{items}</Breadcrumbs>
            <div className="flex flex-col md:flex-row gap-3">
                <Link
                    href="quiz-rooms/create-quiz-room"
                    className="flex h-[40px] bg-[--primary] items-center gap-3 rounded-md py-3 text-white text-sm font-medium justify-start px-3"
                >
                    <PlusIcon className="w-6" />
                    <p className="block">Create Quiz Room</p>
                </Link>
               
                <div className="grow"></div>

                <div className="flex">
                    <SearchField
                        value={searchQuery2}
                        onChange={(e) => {
                            setSearchQuery(e.target.value);
                        }}
                        onKeyDown={(e) => {}}
                    />
                </div>
            </div>
            <QuizRoomTable />
          {/* <Pagination form={form} totalPages={totalPages} /> */}
        </div>
    );
}
