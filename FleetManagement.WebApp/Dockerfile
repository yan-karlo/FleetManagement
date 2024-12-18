FROM node:21
WORKDIR /app
COPY FleetManagement.WebApp/.  .

RUN npm install
RUN npm install @angular/cli -g
EXPOSE 61297
CMD ["ng", "serve", "--host", "0.0.0.0"]
