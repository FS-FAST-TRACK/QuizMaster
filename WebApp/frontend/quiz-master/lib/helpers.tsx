import { UseFormReturnType, useForm } from "@mantine/form";
import {
    Question,
    QuestionCreateDto,
    QuestionCreateValues,
    QuestionDetailCreateDto,
    QuestionValues,
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

export function humanFileSize(bytes?: number, si = true, dp = 1) {
    if (bytes === undefined) {
        return;
    }
    const thresh = si ? 1000 : 1024;

    if (Math.abs(bytes) < thresh) {
        return bytes + " B";
    }

    const units = si
        ? ["KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"]
        : ["KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"];
    let u = -1;
    const r = 10 ** dp;

    do {
        bytes /= thresh;
        ++u;
    } while (
        Math.round(Math.abs(bytes) * r) / r >= thresh &&
        u < units.length - 1
    );

    return bytes.toFixed(dp) + " " + units[u];
}
