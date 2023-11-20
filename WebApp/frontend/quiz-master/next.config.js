/** @type {import('next').NextConfig} */
const nextConfig = {
  env: {
    QUIZMASTER_GATEWAY: process.env.QUIZMASTER_GATEWAY,
    QUIZMASTER_QUIZ: process.env.QUIZMASTER_QUIZ,
  },
};

module.exports = nextConfig;
