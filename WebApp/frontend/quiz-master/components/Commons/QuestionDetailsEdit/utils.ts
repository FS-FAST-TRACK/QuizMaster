import {
    PatchItem,
    QuestionDetail,
    QuestionDetailCreateDto,
} from "@/lib/definitions";
import { UseFormReturnType } from "@mantine/form";

interface QuestionDetailsAndPatches {
    id: number;
    patchRequest: PatchItem[];
}

function isEqual(a: string[], b: string[]) {
    if (a === b) return true;
    if (a == null || b == null) return false;
    if (a.length !== b.length) return false;

    // If you don't care about the order of the elements inside
    // the array, you should sort both arrays here.
    // Please note that calling sort on an array will modify that array.
    // you might want to clone your array first.

    for (var i = 0; i < a.length; ++i) {
        if (a[i] !== b[i]) return false;
    }
    return true;
}

export const getTransformedData = (
    form: UseFormReturnType<{
        details: QuestionDetail[];
    }>,
    oldDetails: QuestionDetail[]
) => {
    var questionDetailsPatchRequest: QuestionDetailsAndPatches[] = [];
    var questionDetailsCreateRequest: QuestionDetailCreateDto[] = [];
    form.values.details.forEach((detail, index) => {
        if (detail.id) {
            const oldDetail = oldDetails.find((q) => q.id === detail.id);
            var patches: PatchItem[] = [];
            // check if there are changes in the description
            if (oldDetail?.qDetailDesc !== detail.qDetailDesc) {
                patches = [
                    ...patches,
                    {
                        op: "replace",
                        value: detail.qDetailDesc,
                        path: "qDetailDesc",
                    },
                ];
            }

            // check if there are changes in the detail types
            if (
                oldDetail &&
                !isEqual(oldDetail.detailTypes, detail.detailTypes)
            ) {
                patches = [
                    ...patches,
                    {
                        op: "replace",
                        value: detail.detailTypes,
                        path: "detailTypes",
                    },
                ];
            }
            if (patches.length > 0) {
                questionDetailsPatchRequest = [
                    ...questionDetailsPatchRequest,
                    {
                        id: detail.id,
                        patchRequest: patches,
                    },
                ];
            }
        }

        if (!detail.id) {
            questionDetailsCreateRequest = [
                ...questionDetailsCreateRequest,
                {
                    qDetailDesc: detail.qDetailDesc,
                    detailTypes: detail.detailTypes,
                },
            ];
        }
    });

    // check if in the form details there are old details that don't have any answer
    var toBeDeleted = oldDetails.filter((old) => {
        if (!form.values.details.map((d) => d.id).includes(old.id)) {
            return old.id;
        }
    });
    var questionDetailsDeleteIds: number[] = toBeDeleted.map((d) => d.id);
    return {
        questionDetailsCreateRequest,
        questionDetailsPatchRequest,
        questionDetailsDeleteIds,
    };
};
