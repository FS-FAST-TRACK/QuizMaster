import { Box, Button, Modal, TextInput } from "@mantine/core";
import {
    Dispatch,
    ReactNode,
    SetStateAction,
    useCallback,
    useEffect,
    useState,
} from "react";
import style from "@/styles/input.module.css";
import { useRouter } from "next/navigation";
import { notifications } from "@mantine/notifications";
import notificationStyles from "../../../styles/notification.module.css";
import { Question, QuestionResourceParameter } from "@/lib/definitions";
import QuestionTable from "../tables/QuestionTable";
import { fetchQuestions } from "@/lib/quizData";
import { useForm } from "@mantine/form";

export type CategoryCreateDto = {
    qCategoryDesc: string;
};
export default function AddQuestionToSetModal({
    onClose,
    opened,
    setQuestions,
    questions,
}: {
    opened: boolean;
    onClose: () => void;
    setQuestions: Dispatch<SetStateAction<Question[]>>;
    questions: Question[];
}) {
    const router = useRouter();
    const [category, setCategory] = useState("");
    const [questionsNotYetAdded, setQuestionsNotYetAdded] = useState<
        Question[]
    >([]);

    const form = useForm<QuestionResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    useEffect(() => {
        if (opened) {
            console.log(questions);
            const fetchQuestion = fetchQuestions({
                questionResourceParameter: form.values,
            });
            fetchQuestion.then((res) => {
                const notYetAddedQuestions = res.data.filter((question) => {
                    return (
                        !questions ||
                        !questions.some((q) => q.id === question.id)
                    );
                });

                setQuestionsNotYetAdded(notYetAddedQuestions);
            });
        }
    }, [opened]);

    const handelSubmit = useCallback(async () => {
        const categoryCreateDto: CategoryCreateDto = {
            qCategoryDesc: category,
        };
        const res = await fetch(
            `${process.env.QUIZMASTER_QUIZ}/api/question/category`,
            {
                method: "POST",
                mode: "cors",
                body: JSON.stringify(categoryCreateDto),
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );

        console.log(res);
        if (res.status === 201) {
            onClose();
            notifications.show({
                color: "green",
                title: "Category created successfully",
                message: "",
                classNames: notificationStyles,
                className: "",
            });
            router.push("/categories");
        } else {
            const error = await res.json();
            notifications.show({
                color: "red",
                title: "Failed to create category",
                message: error.message,
                classNames: notificationStyles,
            });
        }
    }, [category]);
    return (
        <Modal
            zIndex={100}
            opened={opened}
            onClose={onClose}
            padding={40}
            title={<div className="font-bold">Add Questions</div>}
            centered
            size="calc(100vw - 3rem)"
        >
            <div className="space-y-5">
                <QuestionTable
                    questions={questionsNotYetAdded}
                    message={
                        questionsNotYetAdded.length === 0
                            ? "No Questions"
                            : undefined
                    }
                />
                <div className="flex justify-end">
                    <Button
                        variant="transparent"
                        color="gray"
                        onClick={onClose}
                    >
                        Cancel
                    </Button>
                    <Button
                        variant="filled"
                        color="green"
                        disabled={category === ""}
                        onClick={handelSubmit}
                    >
                        Add Category
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
