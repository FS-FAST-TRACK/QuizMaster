import { QuestionCreateValues } from "@/lib/definitions";
import { InputLabel } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function PuzzleQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
    return (
        <div>
            <InputLabel>Choices</InputLabel>
            <div draggable>helo</div>
        </div>
    );
}
