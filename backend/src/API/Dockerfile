FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ClientAcquisition.sln ./
COPY backend/src/API/ClientAcquisition.Api.csproj backend/src/API/
COPY backend/src/Application/ClientAcquisition.Application.csproj backend/src/Application/
COPY backend/src/Domain/ClientAcquisition.Domain.csproj backend/src/Domain/
COPY backend/src/Infrastructure/ClientAcquisition.Infrastructure.csproj backend/src/Infrastructure/

RUN dotnet restore backend/src/API/ClientAcquisition.Api.csproj

COPY . .
RUN dotnet publish backend/src/API/ClientAcquisition.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClientAcquisition.Api.dll"]