import {
    PaginationMetadata,
    QuestionResourceParameter,
} from "@/lib/definitions";
import { Pagination, Select, Text } from "@mantine/core";
import { UseFormReturnType } from "@mantine/form";

const pageSizes = ["10", "20", "30", "40", "50"];

export default function CustomPagination({
    form,
    metadata,
}: {
    form: UseFormReturnType<QuestionResourceParameter>;
    metadata: PaginationMetadata | undefined;
}) {
    return (
        <div className="flex flex-col md:flex-row items-center md:justify-end gap-5 md:gap-10">
            <div className="gap-5 flex items-center justify-center">
                <Text size="sm">Show</Text>
                <Select
                    withCheckIcon={false}
                    data={pageSizes}
                    className="w-16"
                    allowDeselect={false}
                    defaultValue="10"
                    onChange={(value) => {
                        form.setFieldValue("pageSize", value!);
                    }}
                />
            </div>
            <Pagination
                total={metadata?.totalPages || 1}
                siblings={1}
                {...form.getInputProps("pageNumber")}
            />
        </div>
    );
}
