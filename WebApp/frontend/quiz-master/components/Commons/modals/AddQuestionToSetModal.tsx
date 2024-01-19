import { Button, Modal } from "@mantine/core";
import {
    Dispatch,
    SetStateAction,
    useCallback,
    useEffect,
    useState,
} from "react";
import { useRouter } from "next/navigation";
import Pagination from "@/components/Commons/Pagination";
import {
    PaginationMetadata,
    Question,
    QuestionFilterProps,
    ResourceParameter,
} from "@/lib/definitions";
import QuestionTable from "../tables/QuestionTable";
import { useForm } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import { fetchQuestion, fetchQuestions } from "@/lib/hooks/question";

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
    const [questionFilters, setQuestionFilters] = useState<QuestionFilterProps>(
        {
            filterByCategories: [],
            filterByDifficulties: [],
            filterByTypes: [],
        }
    );
    const [paginationMetadata, setPaginationMetadata] = useState<
        PaginationMetadata | undefined
    >();

    const form = useForm<ResourceParameter>({
        initialValues: {
            pageSize: "10",
            searchQuery: "",
            pageNumber: 1,
        },
    });
    const populateQuestions = useCallback(async () => {
        var response = await fetchQuestions({
            questionResourceParameter: {
                ...form.values,
                ...questionFilters,
                exludeQuestionsIds: questions.map((q) => q.id),
            },
        });
        if (response.type === "success") {
            setQuestionsNotYetAdded(response.data?.questions!);
            setPaginationMetadata(response.data?.paginationMetada);
        } else {
        }
    }, [form.values]);
    //Get questions not yet added
    useEffect(() => {
        open();
        if (opened) {
            populateQuestions();
        }
        close();
    }, [form.values, opened]);

    const handelSubmit = useCallback(async () => {
        selectedRows.map(async (row) => {
            await fetchQuestion({ questionId: row }).then((res) => {
                setQuestions((prev) => {
                    const newQuestion = res.data?.question;

                    // Check if newQuestion is defined before updating the state
                    if (newQuestion) {
                        return [...prev, newQuestion];
                    }

                    // If newQuestion is undefined, return the previous state
                    return prev;
                });
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
                    callInQuestionsPage="modal"
                />
                <Pagination form={form} metadata={paginationMetadata} />
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
