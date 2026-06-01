FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ClientAcquisition.sln ./
COPY frontend/src/ClientAcquisition.Frontend.csproj frontend/src/

RUN dotnet restore frontend/src/ClientAcquisition.Frontend.csproj

COPY . .
RUN dotnet publish frontend/src/ClientAcquisition.Frontend.csproj -c Release -o /app/publish

FROM golang:1.23-alpine AS server-build
WORKDIR /server

COPY frontend/src/server/main.go .
RUN go build -o /out/spa-server main.go

FROM scratch AS runtime
WORKDIR /www
COPY --from=build /app/publish/wwwroot ./
COPY --from=server-build /out/spa-server /spa-server
EXPOSE 8080
ENTRYPOINT ["/spa-server", "-root", "/www", "-listen", ":8080"]
