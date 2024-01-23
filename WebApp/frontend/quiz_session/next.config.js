/** @type {import('next').NextConfig} */
const nextConfig = {
  /*
      I have ignored linting and errors for now 
  */
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
