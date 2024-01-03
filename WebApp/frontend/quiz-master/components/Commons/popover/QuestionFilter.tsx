import { QuestionFilterProps } from "@/lib/definitions";
import { useQuestionCategoriesStore } from "@/store/CategoryStore";
import { useQuestionDifficultiesStore } from "@/store/DifficultyStore";
import { useQuestionTypesStore } from "@/store/TypeStore";
import { FunnelIcon } from "@heroicons/react/24/outline";
import { Button, MultiSelect, NumberInput, Popover } from "@mantine/core";
import { useForm } from "@mantine/form";
import { Dispatch, SetStateAction, useState } from "react";

export default function QuestionFilter({
    setQuestionFilters,
}: {
    setQuestionFilters: (filter: QuestionFilterProps) => void;
}) {
    const { questionCategories } = useQuestionCategoriesStore();
    const { questionDifficulties } = useQuestionDifficultiesStore();
    const { questionTypes } = useQuestionTypesStore();

    const form = useForm({
        initialValues: {
            filterByCategories: [],
            filterByDifficulties: [],
            filterByTypes: [],
        },
    });
    return (
        <div>
            <Popover width={300} position="bottom" withArrow shadow="md">
                <Popover.Target>
                    <div className="cursor-pointer w-10 flex items-center justify-center aspect-square">
                        <FunnelIcon className="w-6" />
                    </div>
                </Popover.Target>
                <Popover.Dropdown>
                    <form
                        className="flex flex-col gap-5"
                        onReset={() => {
                            form.reset();
                            setQuestionFilters({
                                filterByCategories: [],
                                filterByDifficulties: [],
                                filterByTypes: [],
                            });
                        }}
                        onSubmit={(e) => {
                            e.preventDefault();
                            setQuestionFilters({
                                filterByCategories:
                                    form.values.filterByCategories.map((c) =>
                                        parseInt(c)
                                    ),
                                filterByDifficulties:
                                    form.values.filterByDifficulties.map((c) =>
                                        parseInt(c)
                                    ),
                                filterByTypes: form.values.filterByTypes.map(
                                    (c) => parseInt(c)
                                ),
                            });
                        }}
                    >
                        <MultiSelect
                            variant="filled"
                            label="Type"
                            placeholder="Choose question types (Max 3)"
                            data={questionTypes.map((cat) => {
                                return {
                                    value: cat.id.toString(),
                                    label: cat.qTypeDesc,
                                };
                            })}
                            comboboxProps={{ withinPortal: false }}
                            clearable
                            maxValues={3}
                            {...form.getInputProps("filterByTypes")}
                        />
                        <MultiSelect
                            variant="filled"
                            label="Category"
                            placeholder="Choose categories (Max 3)"
                            data={questionCategories.map((cat) => {
                                return {
                                    value: cat.id.toString(),
                                    label: cat.qCategoryDesc,
                                };
                            })}
                            comboboxProps={{ withinPortal: false }}
                            clearable
                            {...form.getInputProps("filterByCategories")}
                            maxValues={3}
                        />
                        <MultiSelect
                            variant="filled"
                            label="Difficulty"
                            placeholder="Select difficulties (Max 3)."
                            data={questionDifficulties.map((cat) => {
                                return {
                                    value: cat.id.toString(),
                                    label: cat.qDifficultyDesc,
                                };
                            })}
                            {...form.getInputProps("filterByDifficulties")}
                            comboboxProps={{ withinPortal: false }}
                            clearable
                            maxValues={3}
                        />

                        <div className="flex gap-5 justify-end">
                            <Button type="reset" variant="outline" color="gray">
                                Reset
                            </Button>
                            <Button type="submit" color="green">
                                Filter
                            </Button>
                        </div>
                    </form>
                </Popover.Dropdown>
            </Popover>
        </div>
    );
}
