import {
    QuestionCreateValues,
    QuestionDetail,
    QuestionValues,
} from "@/lib/definitions";
import { patchQuestionDetail } from "@/lib/hooks/questionDetails";
import { CheckCircleIcon } from "@heroicons/react/24/outline";
import { NumberInput, Text } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

export default function SliderQuestionDetails({
    details,
}: {
    details?: QuestionDetail[];
}) {
    const min = details?.find((qDetail) =>
        qDetail.detailTypes.includes("minimum")
    );
    const max = details?.find((qDetail) =>
        qDetail.detailTypes.includes("maximum")
    );
    const interval = details?.find((qDetail) =>
        qDetail.detailTypes.includes("interval")
    );
    const answer = details?.find((qDetail) =>
        qDetail.detailTypes.includes("answer")
    );

    return (
        <>
            <div className="flex [&>*]:basis-1/3 ">
                <div>
                    <p>Slider Minimium</p>
                    <p className="text-xl font-bold">{min?.qDetailDesc}</p>
                </div>
                <div>
                    <p>Slider Interval</p>
                    <p className="text-xl font-bold">{interval?.qDetailDesc}</p>
                </div>
                <div>
                    <p>Slider Maximum</p>
                    <p className="text-xl font-bold">{max?.qDetailDesc}</p>
                </div>
            </div>
            <div
                className={`flex gap-3 border-2
                    
                         border-[var(--primary)] text-[var(--primary)]
                        
                 p-2 items-center rounded-lg  font-semibold`}
            >
                <Text>{answer?.qDetailDesc}</Text>
                <div className="grow"></div>

                <CheckCircleIcon className="w-6 text-white bg-green-600 rounded-full" />
                <Text
                    size="sm"
                    style={{
                        color: "var(--primary)",
                    }}
                >
                    Correct Answer
                </Text>
            </div>
        </>
    );
}
