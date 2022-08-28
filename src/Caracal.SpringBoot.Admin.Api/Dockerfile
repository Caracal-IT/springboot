FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Caracal.SpringBoot.Admin.Api/Caracal.SpringBoot.Admin.Api.csproj", "Caracal.SpringBoot.Admin.Api/"]
RUN dotnet restore "src/Caracal.SpringBoot.Admin.Api/Caracal.SpringBoot.Admin.Api.csproj"
COPY . .
WORKDIR "/src/Caracal.SpringBoot.Admin.Api"
RUN dotnet build "Caracal.SpringBoot.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Caracal.SpringBoot.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Caracal.SpringBoot.Admin.Api.dll"]
