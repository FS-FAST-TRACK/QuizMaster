import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionEdit,
    QuestionValues,
} from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { NumberInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function SliderQuestionDetails({
    form,
}: {
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>;
}) {
    return (
        <div className="flex gap-5">
            <NumberInput
                variant="filled"
                label="Min"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `details.${form.values.details.findIndex((detail) =>
                        detail.detailTypes.includes("minimum")
                    )}.qDetailDesc`
                )}
            />
            <NumberInput
                variant="filled"
                label="Max"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `details.${form.values.details.findIndex((detail) =>
                        detail.detailTypes.includes("maximum")
                    )}.qDetailDesc`
                )}
            />
            <NumberInput
                variant="filled"
                label="Interval"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `details.${form.values.details.findIndex((detail) =>
                        detail.detailTypes.includes("interval")
                    )}.qDetailDesc`
                )}
            />
            <NumberInput
                variant="filled"
                label="Answer"
                withAsterisk
                hideControls
                {...form.getInputProps(
                    `details.${form.values.details.findIndex((detail) =>
                        detail.detailTypes.includes("answer")
                    )}.qDetailDesc`
                )}
            />
        </div>
    );
}
