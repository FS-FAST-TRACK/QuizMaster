import QuestionCreate from "@/components/Commons/QuestionCreate";

export default function Page() {
    return (
        <div className="flex flex-col">
            {/* Page header */}
            <div className="flex flex-col md:flex-row justify-between ">
                <h3>Create New Question</h3>
            </div>
            {/* Page Content */}
            <QuestionCreate />
        </div>
    );
}
