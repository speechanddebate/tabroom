FROM node:18.2.0 AS base
WORKDIR /api
COPY ./ ./

RUN npm i

ENV NODE_ENV=production
ENV PORT=3000
ENV NODE_OPTIONS="--max_old_space_size=8000 --experimental-vm-modules"
CMD NODE_OPTIONS=${NODE_OPTIONS} node --experimental-specifier-resolution=node --use_strict app.js
