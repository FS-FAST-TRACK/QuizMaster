import { ChangeEventHandler, KeyboardEventHandler } from "react";

export default function SearchField({
    value,
    onChange,
    onKeyDown,
}: {
    value?: string;
    onChange: ChangeEventHandler<HTMLInputElement>;
    onKeyDown: KeyboardEventHandler<HTMLInputElement>;
}) {
    return (
        <input
            placeholder="Search"
            className="h-[40px] rounded-lg px-5 focus:outline-green-500"
            value={value}
            onChange={onChange}
            onKeyDown={onKeyDown}
        />
    );
}
