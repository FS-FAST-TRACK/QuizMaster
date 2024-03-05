# FROM node:18-alpine AS base

# # Install dependencies only when needed

# FROM base AS deps
# RUN apk add --no-cache libc6-compat
# WORKDIR /app

# COPY package.json package-lock.json*  ./
# RUN npm install

# # Rebuild the source code only when needed
# FROM base as builder
# WORKDIR /app
# COPY --from=deps /app/node_modules ./node_modules

# # COPY only the necessary environment file for building
# COPY .env .env

# # COPY the rest of the app's code
# COPY . .

# # ENV NEXT_TELEMETRY_DISABLED 1

# RUN npm run build

# # Production Image, copy all the fules and run next
# FROM base as runner
# WORKDIR /app

# ENV NODE_ENV production

# # RUN addgroup --system --gid 1001 nodejs
# # RUN adduser --system --uid 1001 nextjs

# # COPY --from=builder /app/public ./public

# # # Set the correct permission for prerender cache
# # RUN mkdir .next
# # RUN chown nextjs:nodejs .next

# # # Automatically leverage output traces to reduce image size
# # # https://nextjs.org/docs/advanced-features/output-file-tracing
# # COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
# # COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

# # USER nextjs

# EXPOSE 3001

# ENV PORT 3001
# # set hostname to localhost
# ENV HOSTNAME "0.0.0.0"

# # server.js is created by next build from the standalone output
# # https://nextjs.org/docs/pages/api-reference/next-config-js/output
# CMD ["node", "server.js"]

FROM node:18-alpine AS build
# Install dependencies only when needed
# Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
RUN apk add --no-cache libc6-compat
WORKDIR /app
# Copy and install the dependencies for the project
COPY package.json package-lock.json ./
RUN npm ci
# Copy all other project files to working directory
COPY . .
# Run the next build process and generate the artifacts
RUN npm run build

# we are using multi stage build process to keep the image size as small as possible
FROM node:18-alpine
# update and install latest dependencies, add dumb-init package
# add a non root user
RUN apk update && apk upgrade && apk add dumb-init && adduser -D nextuser 

# set work dir as app
WORKDIR /app
# copy the public folder from the project as this is not included in the build process
COPY --from=build --chown=nextuser:nextuser /app/public ./public
# copy the standalone folder inside the .next folder generated from the build process 
COPY --from=build --chown=nextuser:nextuser /app/.next/standalone ./
# copy the static folder inside the .next folder generated from the build process 
COPY --from=build --chown=nextuser:nextuser /app/.next/static ./.next/static
# set non root user
USER nextuser

# expose 3000 on container
EXPOSE 3001

# set app host ,port and node env 
ENV HOST=0.0.0.0 PORT=3001 NODE_ENV=production
# start the app with dumb init to spawn the Node.js runtime process
# with signal support
CMD ["dumb-init","node","server.js"]