FROM node:22.2.0 as build-stage
ENV NODE_OPTIONS=--openssl-legacy-provider
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY ./ .
RUN npm run build

ARG AbonentDataAccountingService_URL
ENV AbonentDataAccountingService_URL="${AbonentDataAccountingService_URL}"
ARG AuthService_URL
ENV AuthService_URL="${AuthService_URL}"
ARG PayService_URL
ENV PayService_URL="${PayService_URL}"
ARG NachislService_URL
ENV NachislService_URL="${NachislService_URL}"
ARG RepairReqService_URL
ENV RepairReqService_URL="${RepairReqService_URL}"

FROM nginx as production-stage
RUN mkdir /app
COPY --from=build-stage /app/dist /app
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]