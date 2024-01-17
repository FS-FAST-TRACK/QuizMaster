import { usePathname, useRouter, useSearchParams } from "next/navigation";

export const useErrorRedirection = ()=>{
   const router = useRouter();
    const searchParams = useSearchParams();
    const pathname = usePathname();

    const redirectToError = () => {
        const params = new URLSearchParams(searchParams);
        params.set("error", "connection-refused");
        router.push(pathname + "?" + params.toString());
    }
    return {
        redirectToError
    }
}
