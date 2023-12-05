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
import { fetchQuestion, fetchQuestions } from "@/lib/quizData";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";

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
    const [questionsNotYetAdded, setQuestionsNotYetAdded] = useState<
        Question[]
    >([]);
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    const [visible, { close, open }] = useDisclosure(true);

    const form = useForm<QuestionResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });

    //Get questions not yet added
    useEffect(() => {
        open();
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
        close();
    }, [form.values, opened]);

    const handelSubmit = useCallback(async () => {
        selectedRows.map(async (row) => {
            await fetchQuestion({ questionId: row }).then((res) => {
                console.log(res.data);
                setQuestions((prev) => [...prev, res.data]);
            });
        });
        onClose();
    }, [selectedRows]);
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
                    setSelectedRow={setSelectedRows}
                    loading={visible}
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
                        disabled={selectedRows.length === 0}
                        onClick={handelSubmit}
                    >
                        Add to set
                    </Button>
                </div>
            </div>
        </Modal>
    );
}
