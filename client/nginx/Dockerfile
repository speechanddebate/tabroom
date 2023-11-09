FROM nginx:1.24.0

COPY nginx.conf /etc/nginx/nginx.conf
RUN rm /etc/nginx/conf.d/default.conf
COPY client.conf /etc/nginx/conf.d/client.conf
