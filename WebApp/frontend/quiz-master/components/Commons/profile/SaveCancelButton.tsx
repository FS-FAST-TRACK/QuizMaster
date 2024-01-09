export default function SaveCancelButton({
    onSave,
    onCancel,
    className,
}: {
    onSave: () => void;
    onCancel?: () => void;
    className?: string;
}) {
    return (
        <div className={`relative flex w-[100%] ${className}`}>
            <button
                className="p-2 bg-[#18A24C] text-white rounded-md mr-2"
                onClick={onSave}
            >
                Save Changes
            </button>
            <button
                className="p-2 border border-black rounded-md text-black"
                onClick={onCancel}
            >
                Cancel
            </button>
        </div>
    );
}
