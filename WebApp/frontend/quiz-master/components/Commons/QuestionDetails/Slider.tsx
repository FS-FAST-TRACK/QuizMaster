import { QuestionCreateValues } from "@/lib/definitions";
import { NumberInput } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function SliderQuestionDetails({
    form,
}: {
    form: UseFormReturnType<QuestionCreateValues>;
}) {
    return (
        <div className="flex gap-5">
            <NumberInput
                variant="filled"
                label="Min"
                withAsterisk
                hideControls
                {...form.getInputProps("minimum")}
            />
            <NumberInput
                variant="filled"
                label="Max"
                withAsterisk
                hideControls
                {...form.getInputProps("maximum")}
            />
            <NumberInput
                variant="filled"
                label="Interval"
                withAsterisk
                hideControls
                {...form.getInputProps("interval")}
            />
            <NumberInput
                variant="filled"
                label="Answer"
                withAsterisk
                hideControls
                {...form.getInputProps("sliderAnswer")}
            />
        </div>
    );
}
