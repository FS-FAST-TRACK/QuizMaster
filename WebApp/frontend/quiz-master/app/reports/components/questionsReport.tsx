"use client";
import { useCallback, useEffect, useState } from "react";
import importLogo from "/public/import-logo.svg";
import pieSample from "/public/pie-sample.svg";
import Image from "next/image";
import {
    PaginationMetadata,
    Question,
    QuestionCategory,
    QuestionDifficulty,
    QuestionFilterProps,
    QuestionType,
    ResourceParameter,
} from "@/lib/definitions";
import { useForm } from "@mantine/form";
import { fetchQuestions } from "@/lib/hooks/question";
import { notification } from "@/lib/notifications";
import { fetchCategories } from "@/lib/quizData";
import { fetchDifficulties } from "@/lib/hooks/difficulty";
import { useDisclosure } from "@mantine/hooks";
import { Checkbox, Select } from "@mantine/core";
import styles from "@/styles/input.module.css";
import { getAllTypes } from "@/lib/hooks/type";

export default function QuestionReport() {
    //Questions Fields and Functions
    const [questions, setQuestions] = useState<Question[]>([]);
    const [questionFilters, setQuestionFilters] = useState<QuestionFilterProps>(
        {
            filterByCategories: [],
            filterByDifficulties: [],
            filterByTypes: [],
        }
    );
    const [qTypes, setQTypes] = useState<QuestionType[]>([]);
    const [paginationMetadata, setPaginationMetadata] = useState<
        PaginationMetadata | undefined
    >();
    const [visible, { close, open }] = useDisclosure(true);

    const form = useForm<ResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });
    useEffect(() => {
        open();
        getQuestions();
        close();
    }, [form.values, questionFilters]);

    const getQuestions = useCallback(async () => {
        try {
            var response = await fetchQuestions({
                questionResourceParameter: {
                    ...form.values,
                    ...questionFilters,
                    exludeQuestionsIds: undefined,
                },
            });
            if (response.type === "success") {
                setQuestions(response.data?.questions!);
                setPaginationMetadata(response.data?.paginationMetada);
            } else {
                notification({
                    type: "error",
                    title: "Failed to fetch questions",
                });
            }

            getAllTypes().then((res) => {
                setQTypes(res.data);
            });
        } catch {
            notification({ type: "error", title: "Something went wrong." });
        }
    }, [form.values, questionFilters]);

    function getType(id: number) {
        return qTypes.find((type) => type.id === id)?.qTypeDesc;
    }

    function qCountPerType(typeId: number) {
        const total = questions.map((q) => q.qTypeId === typeId);

        return total.length;
    }

    //Category Fields and Functions
    const [categories, setCategories] = useState<QuestionCategory[]>([]);
    const [searchQueryCat, setSearchQueryCat] = useState<string>("");

    useEffect(() => {
        var categoriesFetch = fetchCategories(form.values);
        categoriesFetch.then((res) => {
            setCategories(res.data);
            if (res.paginationMetadata !== null) {
                setPaginationMetadata(res.paginationMetadata);
            }
        });
    }, [form.values]);

    //Difficulty Fields and Functions
    const [difficulties, setDifficulties] = useState<QuestionDifficulty[]>([]);
    const [searchQueryDif, setSearchQueryDif] = useState<string>("");

    useEffect(() => {
        try {
            var questionsFetch = fetchDifficulties(form.values);
            questionsFetch.then((res) => {
                setDifficulties(res.data);
                setPaginationMetadata(res.paginationMetadata!);
            });
        } catch (error) {}
    }, [form.values]);

    useEffect(() => {
        console.log("questions list");
        console.log(questions);
    }, [questions]);

    useEffect(() => {
        console.log("categories list");
        console.log(categories);
    }, [categories]);

    useEffect(() => {
        console.log("difficulties list");
        console.log(difficulties);
    }, [difficulties]);

    return (
        <div className="flex flex-col p-8 gap-4 bg-white rounded-lg border shadow-lg">
            <div className="flex flex-row justify-between items-center">
                <p className="text-xl font-bold">Questions</p>
                <div>
                    <Image
                        src={importLogo}
                        alt="Import"
                        width={50}
                        height={50}
                    />
                </div>
            </div>
            <div className="flex lg:flex-row flex-col gap-8 items-center">
                <div>
                    <Image
                        src={pieSample}
                        alt="Graph"
                        width={200}
                        height={200}
                    />
                </div>
                <div className="flex flex-col gap-4">
                    <div className="flex flex-col gap-2">
                        <div>
                            <p className=" font-bold text-sm">Difficulties:</p>
                        </div>
                        <div className="flex flex-row gap-8">
                            {difficulties.map((difficulty) => {
                                return (
                                    <Checkbox
                                        key={difficulty.id}
                                        defaultChecked
                                        label={`${difficulty.qDifficultyDesc}`}
                                        color="gray"
                                    />
                                );
                            })}
                        </div>
                    </div>
                    <div className="flex flex-col gap-2">
                        <div>
                            <p className=" font-bold text-sm">Types:</p>
                        </div>
                        <div className="flex flex-row gap-8 text-sm">
                            {questions.map((question) => {
                                return (
                                    <label key={question.id}>
                                        {`${getType(
                                            question.qTypeId
                                        )} (${qCountPerType(
                                            question.qTypeId
                                        )})`}
                                    </label>
                                );
                            })}
                        </div>
                    </div>
                    <div className="flex flex-col gap-2">
                        <div>
                            <p className=" font-bold text-sm">Categories:</p>
                        </div>
                        <div className="flex flex-row gap-8">
                            <Select
                                variant="filled"
                                placeholder="Choose Category"
                                data={categories.map((cat) => {
                                    return {
                                        value: cat.id.toString(),
                                        label: cat.qCategoryDesc,
                                    };
                                })}
                                {...form.getInputProps("qCategoryId")}
                                clearable
                                classNames={styles}
                                allowDeselect={false}
                            />
                        </div>
                        <p className=" opacity-70 text-xs">
                            {categories.map((cat, index) => {
                                var str = "";
                                if (categories.length === 1) {
                                    return `(${cat.qCategoryDesc})`;
                                } else if (index === 0) {
                                    str += `(${cat.qCategoryDesc}`;
                                } else if (
                                    index >= 1 &&
                                    index < categories.length - 1
                                ) {
                                    str += `, ${cat.qCategoryDesc}`;
                                } else {
                                    str += `, ${cat.qCategoryDesc})`;
                                }

                                return str;
                            })}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
