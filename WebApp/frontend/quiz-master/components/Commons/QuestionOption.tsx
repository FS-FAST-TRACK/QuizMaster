import {
  QuestionCreateValues,
  QuestionDetailCreateDto,
} from "@/lib/definitions";
import { TextInput, Textarea } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function QuestionOption({
  form,
}: {
  form: UseFormReturnType<QuestionCreateValues>;
}) {
  var options = form.values.questionDetailCreateDtos.filter((qDetail) =>
    qDetail.detailTypes.includes("option"),
  );
  console.log(options, form.values.questionDetailCreateDtos);
  if (form.values.qTypeId == "1")
    return (
      <div className="flex flex-row ">
        <div>
          {options.map((option, index) => (
            <TextInput
              {...form.getInputProps("questionDetailCreateDtos")}
              value={option.qDetailDesc}
              placeholder={`Option ${index + 1}`}
            />
          ))}
        </div>
        <div></div>
      </div>
    );
  return <div></div>;
}
