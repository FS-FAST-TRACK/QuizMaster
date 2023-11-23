import { Question } from "@/lib/definitions";

export default function QuesitonCard({ question }: { question?: Question }) {
    if (!question) {
        return;
    }
    return (
        <div className="bg-gray rounded-lg px-5 py-3 ">
            {question.qStatement}
        </div>
    );
}
