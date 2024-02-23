/** @type {import('next').NextConfig} */
const nextConfig = {
  /*
      I have ignored linting and errors for now 
  */
  env: {
    QUIZMASTER_GATEWAY:
      process.env.QUIZMASTER_GATEWAY ??
      process.env.NEXT_PUBLIC_QUIZMASTER_GATEWAY,
    QUIZMASTER_ADMIN:
      process.env.QUIZMASTER_ADMIN ?? process.env.NEXT_PUBLIC_QUIZMASTER_ADMIN,
  },
  eslint: {
    ignoreDuringBuilds: ["/app", "/components", "/.next"],
  },
  typescript: {
    ignoreBuildErrors: true,
  },
  images: {
    domains: ["localhost"],
  },
};

module.exports = nextConfig;
