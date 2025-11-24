FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./OrderService/OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["./OrderService/OrderService.Entity/OrderService.Entity.csproj", "OrderService.Entity/"]
COPY ["./OrderService/OrderService.Event/OrderService.Event.csproj", "OrderService.Event/"]

RUN dotnet restore "OrderService.API/OrderService.API.csproj"

COPY OrderService/ OrderService/

WORKDIR "/src/OrderService/OrderService.API"

RUN dotnet build "./OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
