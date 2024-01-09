import { usePathname, useRouter, useSearchParams } from "next/navigation";
import React, { ReactNode, useCallback } from "react";
import ErrorConnectionRefused from "./ErrorConnectionRefused";
import { Button } from "@mantine/core";
import path from "path";
import ErrorInternal from "./ErrorInternal";

const ErrorContainer = ({ children }: { children: ReactNode }) => {
    const searchParams = useSearchParams();
    const pathname = usePathname();
    const router = useRouter();

    const error = searchParams.get("error");
    const onTryAgain = useCallback(() => {
        const params = new URLSearchParams(searchParams);
        params.delete("error");
        if (params.size === 0) {
            router.push(pathname);
        }
        router.push(pathname + "?" + params);
    }, [error]);

    const errorPagesDict = [
        {
            error: "connection-refused",
            component: <ErrorConnectionRefused />,
        },
        {
            error: "unexpected",
            component: <ErrorInternal />,
        },
    ];
    const errorComponent = errorPagesDict.find((page) => page.error === error)
        ?.component;

    if (error) {
        if (errorComponent) {
            return (
                <div className="w-full h-full flex items-center justify-center [&>*]:w-fit">
                    <div>
                        {errorComponent}

                        <form onSubmit={onTryAgain}>
                            <Button type="submit">Try again</Button>
                        </form>
                    </div>
                </div>
            );
        } else {
            onTryAgain();
        }
    }
    return children;
};

export default ErrorContainer;
