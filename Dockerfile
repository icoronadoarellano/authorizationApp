FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln .
COPY Authorization.API/*.csproj ./Authorization.API/
COPY Authorization.BusinessLogic/*.csproj ./Authorization.BusinessLogic/
COPY Authorization.DataAccess/*.csproj ./Authorization.DataAccess/
COPY Authorization.EntityBusiness/*.csproj ./Authorization.EntityBusiness/
RUN dotnet restore "Authorization.API/Authorization.API.csproj"

COPY Authorization.API/. ./Authorization.API/
COPY Authorization.BusinessLogic/. ./Authorization.BusinessLogic/
COPY Authorization.DataAccess/. ./Authorization.DataAccess/
COPY Authorization.EntityBusiness/. ./Authorization.EntityBusiness/
WORKDIR /src/Authorization.API
RUN dotnet build "Authorization.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authorization.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","Authorization.API.dll","--environment=Production"]