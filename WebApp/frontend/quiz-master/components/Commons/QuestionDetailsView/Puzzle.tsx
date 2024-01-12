import { QuestionDetail } from "@/lib/definitions";
import { InputLabel } from "@mantine/core";

export default function PuzzleQuestionDetails({
    details,
}: {
    details?: QuestionDetail[];
}) {
    return (
        <div className="flex flex-col gap-2 max-w-96">
            <InputLabel>Items Arrange in the Correct Order</InputLabel>
            {details?.map((item, index) => {
                if (!item.detailTypes.includes("option")) {
                    return;
                }
                return (
                    <div
                        key={index}
                        className="rounded border p-4 border-[var(--primary)] text-[var(--primary)] font-bold"
                    >
                        {item.qDetailDesc}
                    </div>
                );
            })}
        </div>
    );
}
