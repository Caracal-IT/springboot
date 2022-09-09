﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /
COPY . .
RUN dotnet restore
RUN dotnet build SpringBoot.sln -c Release -o /app/build -f:net7.0

FROM build AS publish
RUN dotnet publish src/Caracal.SpringBoot.Api/Caracal.SpringBoot.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Caracal.SpringBoot.Api.dll"]
