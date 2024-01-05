import { Question } from "@/lib/definitions";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { EllipsisVerticalIcon, TrashIcon } from "@heroicons/react/24/outline";
import {
    Box,
    Checkbox,
    Loader,
    LoadingOverlay,
    Popover,
    Table,
    Text,
} from "@mantine/core";
import {
    Dispatch,
    SetStateAction,
    useCallback,
    useEffect,
    useState,
} from "react";
import QuesitonAction from "../popover/QuestionAction";
import PromptModal from "../modals/PromptModal";
import QuesitonCard from "../cards/QuestionCard";
import QuestionModal from "../modals/ViewQuestionModal";
import ViewQuestionModal from "../modals/ViewQuestionModal";
import { deleteQuestion as removeQuestion } from "@/lib/hooks/question";
import { QUIZMASTER_QUESTION_DELETE } from "@/api/api-routes";
import { notification } from "@/lib/notifications";

export default function QuestionTable({
    questions,
    message,
    setSelectedRow,
    loading,
    callInQuestionsPage,
}: {
    questions: Question[];
    message?: string;
    setSelectedRow: Dispatch<SetStateAction<number[]>>;
    loading: boolean;
    callInQuestionsPage: string | "";
}) {
    const { getQuestionCategoryDescription } = useQuestionCategoriesStore();
    const { getQuestionDifficultyDescription } = useQuestionDifficultiesStore();
    const { getQuestionTypeDescription } = useQuestionTypesStore();

    const [deleteQuestion, setDeleteQuestion] = useState<Question>();
    const [selectedRows, setSelectedRows] = useState<number[]>([]);
    const [viewQuestion, setViewQuestion] = useState<Question | undefined>();

    useEffect(() => {
        setSelectedRows([]);
    }, [questions]);

    useEffect(() => {
        setSelectedRow(selectedRows);
    }, [selectedRows]);

    const handelDelete = useCallback(async () => {
        if (deleteQuestion) {
            try {
                const res = await removeQuestion({ id: deleteQuestion.id });
                if (res.type === "success") {
                    notification({ type: "success", title: res.message });
                } else {
                    notification({ type: "error", title: res.message });
                }
                setDeleteQuestion(undefined);
            } catch (error) {
                notification({ type: "error", title: "Something went wrong" });
            }
        }
    }, [deleteQuestion]);

    const rows = questions.map((question) => (
        <Table.Tr
            key={question.id}
            bg={
                selectedRows.includes(question.id)
                    ? "var(--primary-100)"
                    : undefined
            }
        >
            <Table.Td>
                <Checkbox
                    color="green"
                    aria-label="Select row"
                    checked={selectedRows.includes(question.id)}
                    onChange={(event) =>
                        setSelectedRows(
                            event.currentTarget.checked
                                ? [...selectedRows, question.id]
                                : selectedRows.filter(
                                      (id) => id !== question.id
                                  )
                        )
                    }
                />
            </Table.Td>
            <Table.Td
                className="cursor-pointer"
                onClick={() => setViewQuestion(question)}
            >
                {question.qStatement}
            </Table.Td>
            <Table.Td>{getQuestionTypeDescription(question.qTypeId)}</Table.Td>
            <Table.Td>
                {getQuestionCategoryDescription(question.qCategoryId)}
            </Table.Td>
            <Table.Td>
                {getQuestionDifficultyDescription(question.qDifficultyId)}
            </Table.Td>
            {callInQuestionsPage === "questions" ? (
                <Table.Td>
                    <QuesitonAction
                        questionId={question.id}
                        onDelete={() => {
                            setDeleteQuestion(question);
                        }}
                    />
                </Table.Td>
            ) : callInQuestionsPage === "set" ? (
                <Table.Td>
                    <Popover width={140} zIndex={10} position="bottom">
                        <Popover.Target>
                            <div className="cursor-pointer flex items-center justify-center aspect-square">
                                <EllipsisVerticalIcon className="w-6" />
                            </div>
                        </Popover.Target>
                        <Popover.Dropdown p={10} className="space-y-3">
                            <button
                                className="flex w-full p-2 gap-2 text-[var(--error)] rounded-lg hover:text-white hover:bg-[var(--error)]   "
                                onClick={() => {
                                    setSelectedRow([question.id]);
                                }}
                            >
                                <TrashIcon className="w-6" />
                                <div>Remove</div>
                            </button>
                        </Popover.Dropdown>
                    </Popover>
                </Table.Td>
            ) : null}
        </Table.Tr>
    ));

    return (
        <div className="w-full border-2 rounded-xl overflow-x-auto grow bg-white">
            <Table striped>
                <Table.Thead>
                    <Table.Tr>
                        <Table.Th>
                            <Checkbox
                                color="green"
                                aria-label="Select row"
                                checked={
                                    selectedRows.length === questions.length
                                }
                                onChange={(event) =>
                                    setSelectedRows(
                                        event.currentTarget.checked
                                            ? questions.map(
                                                  (question) => question.id
                                              )
                                            : []
                                    )
                                }
                            />
                        </Table.Th>
                        <Table.Th>Question Statement</Table.Th>
                        <Table.Th>Type</Table.Th>
                        <Table.Th>Category</Table.Th>
                        <Table.Th>Difficulty</Table.Th>
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {loading ? (
                        <Table.Tr>
                            <Table.Td colSpan={99} rowSpan={10}>
                                <div className="relative h-60">
                                    <LoadingOverlay visible={loading} />
                                </div>
                            </Table.Td>
                        </Table.Tr>
                    ) : questions.length === 0 ? (
                        <Table.Tr>
                            <Table.Td colSpan={99} rowSpan={10}>
                                <div className="flex grow justify-center">
                                    {message ? (
                                        message
                                    ) : (
                                        <Loader size={50} color="green" />
                                    )}
                                </div>
                            </Table.Td>
                        </Table.Tr>
                    ) : (
                        rows
                    )}
                </Table.Tbody>
            </Table>
            <PromptModal
                body={
                    <div>
                        <Text>Are you sure want to delete.</Text>
                        <QuesitonCard question={deleteQuestion} />
                    </div>
                }
                action="Delete"
                onConfirm={handelDelete}
                opened={deleteQuestion ? true : false}
                onClose={() => {
                    setDeleteQuestion(undefined);
                }}
                title="Delete Question"
            />

            <ViewQuestionModal
                opened={viewQuestion !== undefined}
                onClose={() => {
                    setViewQuestion(undefined);
                }}
                question={viewQuestion}
                callInQuestionsPage={callInQuestionsPage}
            />
        </div>
    );
}
