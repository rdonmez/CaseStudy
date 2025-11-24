FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./StockService/StockService.API/StockService.API.csproj", "StockService.API/"]
COPY ["./StockService/StockService.Entity/StockService.Entity.csproj", "StockService.Entity/"]
COPY ["./StockService/StockService.Event/StockService.Event.csproj", "StockService.Event/"]

RUN dotnet restore "StockService.API/StockService.API.csproj"

COPY StockService/ StockService/

WORKDIR "/src/StockService/StockService.API"

RUN dotnet build "./StockService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StockService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockService.API.dll"]
