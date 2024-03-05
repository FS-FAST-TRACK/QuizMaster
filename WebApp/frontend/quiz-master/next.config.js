/** @type {import('next').NextConfig} */
const nextConfig = {
    env: {
        QUIZMASTER_GATEWAY:
            process.env.QUIZMASTER_GATEWAY ??
            process.env.NEXT_PUBLIC_QUIZMASTER_GATEWAY,
        QUIZMASTER_QUIZ:
            process.env.QUIZMASTER_QUIZ &&
            process.env.NEXT_PUBLIC_QUIZMASTER_QUIZ,
        QUIZMASTER_MEDIA:
            process.env.QUIZMASTER_MEDIA &&
            process.env.NEXT_PUBLIC_QUIZMASTER_MEDIA,
        NEXTAUTH_SECRET:
            process.env.NEXTAUTH_SECRET &&
            process.env.NEXT_PUBLIC_NEXTAUTH_SECRET,
        QUIZMASTER_SESSION_WEBSITE:
            process.env.QUIZMASTER_SESSION &&
            process.env.NEXT_PUBLIC_QUIZMASTER_SESSION,
        QUIZMASTER_MONITORING:
            process.env.QUIZMASTER_MONITORING &&
            process.env.NEXT_PUBLIC_QUIZMASTER_MONITORING,
        BASE_URL: process.env.BASE_URL && process.env.NEXT_PUBLIC_BASE_URL
    },
    /*
      I have ignored linting and errors for now 
    */
    experimental: { serverActions: true },
    output: "standalone"
};

module.exports = nextConfig;
