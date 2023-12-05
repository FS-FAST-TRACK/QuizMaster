import { PropsWithChildren } from "react";

const RegisterLayout = ({ children }: PropsWithChildren) => {
    return (
        <div className="flex min-h-screen flex-col items-center justify-center h-screen p-24 bg-gradient-to-tr from-30% from-[#17A14B] to-[#1BD260]">
            {children}
        </div>
    );
};

export default RegisterLayout;
