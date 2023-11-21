export default function PageHeader({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <div className="text-xl font-bold text-white bg-gradient-to-r from-[#17A14B] to-[#1AC059] px-10 py-8 flex flex-col md:flex-row justify-between ">
            {children}
        </div>
    );
}
