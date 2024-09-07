FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["nuget.config", ""]
COPY ["src/EShop.API/EShop.API.csproj", "src/EShop.API/"]
RUN dotnet restore --configfile "./nuget.config" "src/EShop.API/EShop.API.csproj"
COPY . .
WORKDIR "/src/src/EShop.API"
RUN dotnet build "EShop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShop.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShop.API.dll"]