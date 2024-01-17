/** @type {import('next').NextConfig} */
const nextConfig = {
    env: {
        QUIZMASTER_GATEWAY: process.env.QUIZMASTER_GATEWAY,
        QUIZMASTER_QUIZ: process.env.QUIZMASTER_QUIZ,
        QUIZMASTER_MEDIA: process.env.QUIZMASTER_MEDIA,
        NEXTAUTH_SECRET: process.env.NEXTAUTH_SECRET,
    },
    /*
      I have ignored linting and errors for now 
    */
    eslint: {
        ignoreDuringBuilds: ["/app", "/components", "/.next"],
    },
    typescript: {
        ignoreBuildErrors: true,
    },
};

module.exports = nextConfig;
