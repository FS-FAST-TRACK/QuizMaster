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
    QUIZMASTER_TRIGGER_SFX_SECONDS:
      process.env.QUIZMASTER_TRIGGER_SFX_SECONDS ??
      process.env.NEXT_PUBLIC_QUIZMASTER_TRIGGER_SFX_SECONDS,
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
  output: "standalone",
};

module.exports = nextConfig;
