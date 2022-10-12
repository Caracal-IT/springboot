FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Caracal.SpringBoot.Admin.Api/Caracal.SpringBoot.Admin.Api.csproj", "src/Caracal.SpringBoot.Admin.Api/"]
COPY ["src/libs/Caracal.Web.MediatR/Caracal.Web.MediatR.csproj", "src/libs/Caracal.Web.MediatR/"]
COPY ["src/libs/Caracal.SpringBoot.Application/Caracal.SpringBoot.Application.csproj", "src/libs/Caracal.SpringBoot.Application/"]
COPY ["src/libs/Caracal.Web.Core/Caracal.Web.Core.csproj", "src/libs/Caracal.Web.Core/"]
COPY ["src/libs/Caracal.SpringBoot.Templates/Caracal.SpringBoot.Templates.csproj", "src/libs/Caracal.SpringBoot.Templates/"]
COPY ["src/libs/Caracal.SpringBoot.Data/Caracal.SpringBoot.Data.csproj", "src/libs/Caracal.SpringBoot.Data/"]
COPY ["src/libs/Caracal.SpringBoot.Data.Postgres/Caracal.SpringBoot.Data.Postgres.csproj", "src/libs/Caracal.SpringBoot.Data.Postgres/"]
RUN dotnet restore "src/Caracal.SpringBoot.Admin.Api/Caracal.SpringBoot.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/Caracal.SpringBoot.Admin.Api"
RUN dotnet build "Caracal.SpringBoot.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Caracal.SpringBoot.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Caracal.SpringBoot.Admin.Api.dll"]
