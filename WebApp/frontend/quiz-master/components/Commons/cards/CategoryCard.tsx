import { QuestionCategory } from "@/lib/definitions";

export function CategoryBody({ category }: { category?: QuestionCategory }) {
    return (
        <div className="flex flex-col gap-5 md:flex-row md:[&>*]:basis-1/3 ">
            <div>
                <p>Date Created</p>
                <p className="text-xl font-bold">
                    {category?.dateCreated.toDateString()}
                </p>
            </div>
            <div>
                <p>Last Modified</p>
                <p className="text-xl font-bold">
                    {category?.dateUpdated.toDateString()}
                </p>
            </div>
            <div>
                <p>Questions</p>
                <p className="text-xl font-bold">{category?.questionCounts}</p>
            </div>
        </div>
    );
}
