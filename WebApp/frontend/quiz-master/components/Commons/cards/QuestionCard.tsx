import { Question } from "@/lib/definitions";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { Chip } from "@mantine/core";

export default function QuesitonCard({ question }: { question?: Question }) {
    const { getQuestionCategoryDescription } = useQuestionCategoriesStore();
    const { getQuestionDifficultyDescription } = useQuestionDifficultiesStore();
    const { getQuestionTypeDescription } = useQuestionTypesStore();

    if (!question) {
        return;
    }
    return (
        <div className="space-y-8">
            <div>
                <div className="flex w-full justify-center">
                    <Chip color="rgba(0, 0, 0, 1)" variant="filled" checked>
                        {question &&
                            getQuestionTypeDescription(question.qTypeId)}
                    </Chip>
                </div>
            </div>
            <div>
                <p>Question Statement</p>
                <p className="text-xl font-bold">{question?.qStatement}</p>
            </div>
            <div className="flex [&>*]:basis-1/3 ">
                <div>
                    <p>Difficulty</p>
                    <p className="text-xl font-bold">
                        {question &&
                            getQuestionDifficultyDescription(
                                question?.qDifficultyId
                            )}
                    </p>
                </div>
                <div>
                    <p>Category</p>
                    <p className="text-xl font-bold">
                        {question &&
                            getQuestionCategoryDescription(
                                question?.qCategoryId
                            )}
                    </p>
                </div>
                <div>
                    <p>Time Limit</p>
                    <p className="text-xl font-bold">{question?.qTime}</p>
                </div>
            </div>
        </div>
    );
}
