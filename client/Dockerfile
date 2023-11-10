FROM node:18.17.1 AS base
WORKDIR /app
COPY ./ ./

ENV NODE_ENV=production
RUN npm ci
RUN npm run build

CMD node -r dotenv/config build/index.js
