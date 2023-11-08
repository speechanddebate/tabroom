FROM node:18.17.1 AS base
WORKDIR /api
COPY ./ ./

RUN npm i

ENV NODE_ENV=production
ENV PORT=3000
ENV NODE_OPTIONS="--max_old_space_size=200 --experimental-vm-modules --experimental-specifier-resolution=node"
CMD NODE_OPTIONS=${NODE_OPTIONS} node --use_strict app.js
