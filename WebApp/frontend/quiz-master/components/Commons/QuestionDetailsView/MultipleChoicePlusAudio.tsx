import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionValues,
} from "@/lib/definitions";
import { InputLabel, Select, Text, TextInput, Title } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";
import MultipleChoiceQuestionDetail from "./MultipleChoice";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";

export default function MultipleChoicePlusAudioQuestionDetail({
    details,
}: {
    details?: QuestionDetail[];
}) {
    return (
        <>
            <div>
                <Text size="sm">Text to be Translated</Text>

                <div className="mb-5 font-bold text-xl">
                    <p>
                        {
                            details?.find((detail) =>
                                detail.detailTypes.includes("textToAudio")
                            )?.qDetailDesc
                        }
                    </p>
                    <p className="text-sm font-medium">
                        [
                        {
                            details?.find((detail) =>
                                detail.detailTypes.includes("language")
                            )?.qDetailDesc
                        }
                        ]
                    </p>
                </div>
            </div>
            <MultipleChoiceQuestionDetail details={details} />
        </>
    );
}
