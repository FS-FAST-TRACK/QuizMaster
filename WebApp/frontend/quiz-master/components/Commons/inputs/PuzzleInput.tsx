import { Bars4Icon, TrashIcon } from "@heroicons/react/24/outline";
import { Input, Text, Tooltip } from "@mantine/core";
import styles from "@/styles/input.module.css";

export default function PuzzleInput({
    onRemove,
    value,
    onChange,
    checked,
    error,
    onFocus,
    onBlur,
}: {
    onRemove: () => void;
    value: any;
    onChange: any;
    checked?: any;
    error?: any;
    onFocus?: any;
    onBlur?: any;
}) {
    return (
        <>
            <Input
                size="lg"
                leftSection={<Bars4Icon className="w-6" />}
                classNames={styles}
                rightSectionWidth={40}
                rightSection={
                    <Tooltip label="Remove">
                        <TrashIcon
                            className="w-6 cursor-pointer"
                            onClick={onRemove}
                        />
                    </Tooltip>
                }
                leftSectionPointerEvents="visible"
                rightSectionPointerEvents="visible"
                value={value}
                onChange={onChange}
                checked={checked}
                error={error}
                onFocus={onFocus}
                onBlur={onBlur}
            />
            {error && (
                <Text size="xs" variant="text" c="red">
                    {error}
                </Text>
            )}
        </>
    );
}
