import { Dispatch, SetStateAction } from "react";

export default function EditField({
    editting,
    title,
    value,
    onInput,
}: {
    editting: Boolean;
    title: string;
    value: string;
    onInput: Dispatch<SetStateAction<string>>;
}) {
    return (
        <div className="relative w-[100%]">
            <p className={`${editting && "font-bold"}`}>{title}</p>
            {!editting && (
                <p className={`text-[18px] font-bold py-2`}>{value}</p>
            )}
            {editting && (
                <input
                    className="border border-black-200 rounded outline-1 p-2 w-[100%]"
                    type="text"
                    onChange={(e) => onInput(e.target.value)}
                    value={`${value}`}
                />
            )}
        </div>
    );
}
