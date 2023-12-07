import { QuestionDifficulty } from "@/lib/definitions";

export function DifficultyCardBody({
    difficulty,
}: {
    difficulty?: QuestionDifficulty;
}) {
    return (
        <div className="flex flex-col gap-5 md:flex-row md:[&>*]:basis-1/3 ">
            <div>
                <p>Date Created</p>
                <p className="text-xl font-bold">
                    {difficulty?.dateCreated.toDateString()}
                </p>
            </div>
            <div>
                <p>Last Modified</p>
                <p className="text-xl font-bold">
                    {difficulty?.dateUpdated.toDateString()}
                </p>
            </div>
            <div>
                <p>Questions</p>
                <p className="text-xl font-bold">
                    {difficulty?.questionCounts}
                </p>
            </div>
        </div>
    );
}
