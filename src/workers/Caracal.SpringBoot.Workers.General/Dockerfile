FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/workers/Caracal.SpringBoot.Workers.General/Caracal.SpringBoot.Workers.General.csproj", "src/workers/Caracal.SpringBoot.Workers.General/"]
COPY ["src/libs/Caracal.SpringBoot.Data.Postgres/Caracal.SpringBoot.Data.Postgres.csproj", "src/libs/Caracal.SpringBoot.Data.Postgres/"]
COPY ["src/libs/Caracal.SpringBoot.Kafka/Caracal.SpringBoot.Kafka.csproj", "src/libs/Caracal.SpringBoot.Kafka/"]
COPY ["src/libs/Caracal.Web.Core/Caracal.Web.Core.csproj", "src/libs/Caracal.Web.Core/"]
RUN dotnet restore "src/workers/Caracal.SpringBoot.Workers.General/Caracal.SpringBoot.Workers.General.csproj"
COPY . .
WORKDIR "/src/src/workers/Caracal.SpringBoot.Workers.General"
RUN dotnet build "Caracal.SpringBoot.Workers.General.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Caracal.SpringBoot.Workers.General.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Caracal.SpringBoot.Workers.General.dll"]
