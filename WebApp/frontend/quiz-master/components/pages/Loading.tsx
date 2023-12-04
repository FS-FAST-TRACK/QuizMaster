import { Loader } from "@mantine/core";

export default function LoadingPage() {
    return (
        <div className="w-full h-full flex items-center justify-center">
            <Loader size={50} />
        </div>
    );
}
