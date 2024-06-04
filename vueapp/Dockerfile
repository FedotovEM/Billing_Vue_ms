FROM node:18.13.0 as build
WORKDIR /app
COPY package*.json ./
COPY vue.config.js ./
COPY . .

RUN npm install
RUN npm run build

FROM node:18.13.0
RUN apt-get update && apt-get install -y nginx
WORKDIR /app
COPY --from=build /app .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]