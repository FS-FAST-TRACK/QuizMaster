import { UseFormReturnType } from "@mantine/form";
import {
    QuestionCreateDto,
    QuestionCreateValues,
    QuestionDetailCreateDto,
} from "./definitions";
import {
    MultipleChoiceData,
    MultipleChoicePlusAudioData,
    PuzzleData,
    SliderData,
    TrueORFalseData,
    TypeAnswerData,
} from "./questionTypeData";

export function mapData(
    form: UseFormReturnType<QuestionCreateValues>
): QuestionCreateDto {
    var questionCreateDto: QuestionCreateDto = {
        qAudio: form.values.qAudio || "nothing",
        qImage: form.values.qImage || "nothing",
        qStatement: form.values.qStatement,
        qTime: parseInt(form.values.qTime),
        qCategoryId: parseInt(form.values.qCategoryId),
        qDifficultyId: parseInt(form.values.qDifficultyId),
        qTypeId: parseInt(form.values.qTypeId),
        questionDetailCreateDtos: form.values.options.map((op) => {
            var dTypes = ["option"];
            if (op.isAnswer) {
                dTypes = dTypes.concat(["answer"]);
            }
            return {
                qDetailDesc: op.value,
                detailTypes: dTypes,
            };
        }),
    };

    switch (questionCreateDto.qTypeId) {
        case MultipleChoiceData.id:
            questionCreateDto.questionDetailCreateDtos =
                form.values.options.map((op) => {
                    var dTypes = ["option"];
                    if (op.isAnswer) {
                        dTypes = dTypes.concat(["answer"]);
                    }
                    return {
                        qDetailDesc: op.value,
                        detailTypes: dTypes,
                    };
                });
            break;
        case MultipleChoicePlusAudioData.id:
            questionCreateDto.questionDetailCreateDtos =
                form.values.options.map((op) => {
                    var dTypes = ["option"];
                    if (op.isAnswer) {
                        dTypes = dTypes.concat(["answer"]);
                    }
                    return {
                        qDetailDesc: op.value,
                        detailTypes: dTypes,
                    };
                });
            questionCreateDto.questionDetailCreateDtos =
                questionCreateDto.questionDetailCreateDtos.concat([
                    {
                        qDetailDesc: form.values.textToAudio!,
                        detailTypes: ["textToAudio"],
                    },
                    {
                        qDetailDesc: form.values.language!,
                        detailTypes: ["language"],
                    },
                ]);
            break;
        case TrueORFalseData.id:
            questionCreateDto.questionDetailCreateDtos = [
                {
                    qDetailDesc: form.values.trueOrFalseAnswer
                        ? "true"
                        : "false",
                    detailTypes: ["answer"],
                },
            ];
            break;
        case TypeAnswerData.id:
            questionCreateDto.questionDetailCreateDtos = [
                {
                    qDetailDesc: form.values.typeAnswer!,
                    detailTypes: ["answer"],
                },
            ];
            break;

        case SliderData.id:
            questionCreateDto.questionDetailCreateDtos = [
                {
                    qDetailDesc: form.values.minimum!.toString(),
                    detailTypes: ["minimum"],
                },
                {
                    qDetailDesc: form.values.maximum!.toString(),
                    detailTypes: ["maximum"],
                },
                {
                    qDetailDesc: form.values.interval!.toString(),
                    detailTypes: ["interval"],
                },
                {
                    qDetailDesc: form.values.sliderAnswer!.toString(),
                    detailTypes: ["answer"],
                },
            ];
            break;
        case PuzzleData.id:
            questionCreateDto.questionDetailCreateDtos =
                form.values.options.map((op) => {
                    var dTypes = ["answer"];

                    return {
                        qDetailDesc: op.value,
                        detailTypes: dTypes,
                    };
                });
            break;
    }
    return questionCreateDto;
}
