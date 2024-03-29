export { default } from "next-auth/middleware";

export const config = {
    // https://nextjs.org/docs/app/building-your-application/routing/middleware#matcher
    matcher: ["/((?!api|_next/static|_next/image|favicon.ico|auth|system-info|contact-us|feedback).*)"],
};
