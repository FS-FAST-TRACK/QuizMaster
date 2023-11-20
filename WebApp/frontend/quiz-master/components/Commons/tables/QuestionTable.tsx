import { Question } from "@/lib/definitions";
import { fetchQuestions } from "@/lib/quizData";
import { questionTableColumns } from "@/lib/tableColumns";
import { useEffect, useState } from "react";

export default function QuestionTable() {
    const [questions, setQuestions] = useState<Question[]>([]);
    useEffect(() => {
        var questionsFetch = fetchQuestions();
        questionsFetch.then((res) => {
            setQuestions(res);
        });
    }, []);

    return (
        <div className="w-full border-2 rounded-xl overflow-x-auto">
            <table className="w-full ">
                <thead>
                    <tr className="table-row font-bold text-black border-b border">
                        {questionTableColumns.map((column, index) => (
                            <th
                                key={index}
                                className={`table-cell py-[10px] ${
                                    column.className ? column.className : ""
                                }`}
                            >
                                {column.label}
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {questions.map((question, index) => (
                        <>
                            <tr key={index} className="table-row bg-white ">
                                {questionTableColumns.map((column, index2) => (
                                    <td
                                        key={"index2" + index2}
                                        className={`px-2 py-2 table-cell ${
                                            column.className && column.className
                                        }`}
                                    >
                                        {column.Render
                                            ? column.Render({ value: question })
                                            : (question as any)[column.key]}
                                    </td>
                                ))}
                                <td></td>
                            </tr>
                        </>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
