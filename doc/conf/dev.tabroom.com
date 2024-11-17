
server {
    listen 80;
    listen [::]:80;
    server_name mason.dev.tabroom.com mason.dev;
    return 301 https://mason.dev.tabroom.com$request_uri;
}

server {
    root /www/tabroom;
    server_name mason.dev mason.dev.tabroom.com;

    location / {
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header Host $http_host;
        proxy_pass http://127.0.0.1:8000;
    }

    location /v1 {
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header Host $http_host;
        proxy_pass http://127.0.0.1:8001/v1;
    }

	access_log /var/log/nginx/mason-dev-access.log;
	error_log  /var/log/nginx/mason-dev-error.log;

    client_max_body_size 50m;

    listen [::]:443 ssl;
    listen 443 ssl;
    ssl_certificate /etc/ssl/certs/nsda/tabroom.com_ecc/fullchain.cer;
    ssl_certificate_key /etc/ssl/certs/nsda/tabroom.com_ecc/tabroom.com.key;
    include /etc/ssl/certs/nsda/options-ssl-nginx.conf;
    ssl_dhparam /etc/ssl/certs/nsda/ssl-dhparams.pem;
}

server {
    listen 80;
    listen [::]:80;
    server_name legacyapi.dev.tabroom.com mason.dev;
    return 301 https://legacyapi.dev.tabroom.com$request_uri;
}

server {
    root /www/legacy-indexcards;
    server_name legacyapi.dev.tabroom.com legacyapi.dev;

    location /v1 {
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header Host $http_host;
        proxy_pass http://127.0.0.1:8001/v1;
    }

	access_log /var/log/nginx/legacyapi-dev-access.log;
	error_log  /var/log/nginx/legacyapi-dev-error.log;

    client_max_body_size 50m;

    listen [::]:443 ssl;
    listen 443 ssl;
    ssl_certificate /etc/ssl/certs/nsda/tabroom.com_ecc/fullchain.cer;
    ssl_certificate_key /etc/ssl/certs/nsda/tabroom.com_ecc/tabroom.com.key;
    include /etc/ssl/certs/nsda/options-ssl-nginx.conf;
    ssl_dhparam /etc/ssl/certs/nsda/ssl-dhparams.pem;
}

server {
    listen 80;
    listen [::]:80;
    server_name schemats.dev.tabroom.com mason.dev;
    return 301 https://schemats.dev.tabroom.com$request_uri;
}

server {
    root /www/schemats;
    server_name schemats.dev.tabroom.com schemats.dev;

    location / {
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header Host $http_host;
        proxy_pass http://127.0.0.1:9001;
    }

    location /v1 {
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header Host $http_host;
        proxy_pass http://127.0.0.1:9002/v1;
    }

	access_log /var/log/nginx/schemats-dev-access.log;
	error_log  /var/log/nginx/schemats-dev-error.log;

    client_max_body_size 50m;

    listen [::]:443 ssl;
    listen 443 ssl;
    ssl_certificate /etc/ssl/certs/nsda/tabroom.com_ecc/fullchain.cer;
    ssl_certificate_key /etc/ssl/certs/nsda/tabroom.com_ecc/tabroom.com.key;
    include /etc/ssl/certs/nsda/options-ssl-nginx.conf;
    ssl_dhparam /etc/ssl/certs/nsda/ssl-dhparams.pem;

}

server {
    listen 80;
    listen [::]:80;
    server_name api.dev.tabroom.com mason.dev;
    return 301 https://api.dev.tabroom.com$request_uri;
}

server {
    root /www/indexcards;
    server_name api.dev.tabroom.com api.dev;

    location /v1 {
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header Host $http_host;
        proxy_pass http://127.0.0.1:9001/v1;
    }

	access_log /var/log/nginx/api-dev-access.log;
	error_log  /var/log/nginx/api-dev-error.log;

    client_max_body_size 50m;

    listen [::]:443 ssl;
    listen 443 ssl;
    ssl_certificate /etc/ssl/certs/nsda/tabroom.com_ecc/fullchain.cer;
    ssl_certificate_key /etc/ssl/certs/nsda/tabroom.com_ecc/tabroom.com.key;
    include /etc/ssl/certs/nsda/options-ssl-nginx.conf;
    ssl_dhparam /etc/ssl/certs/nsda/ssl-dhparams.pem;
}

server {
    listen 9003;

    location / {
        proxy_pass http://127.0.0.1:9002;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    listen [::]:443 ssl;
    listen 443 ssl;
    ssl_certificate /etc/ssl/certs/nsda/tabroom.com_ecc/fullchain.cer;
    ssl_certificate_key /etc/ssl/certs/nsda/tabroom.com_ecc/tabroom.com.key;
    include /etc/ssl/certs/nsda/options-ssl-nginx.conf;
    ssl_dhparam /etc/ssl/certs/nsda/ssl-dhparams.pem;
}

