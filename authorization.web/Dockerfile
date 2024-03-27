FROM node:20.10.0
WORKDIR /app
COPY package*.json ./
COPY . .
RUN npm install

ARG VITE_API
# expose the variable to the finished cotainer
ENV VITE_API=$VITE_API

RUN npm run build
EXPOSE 5173
CMD [ "npm", "run", "preview" ]