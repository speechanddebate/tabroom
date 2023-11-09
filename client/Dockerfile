FROM node:18.17.1 AS base
WORKDIR /app
COPY ./ ./

RUN npm i
RUN npm run build

ENV NODE_ENV=production
CMD node -r dotenv/config build/index.js
