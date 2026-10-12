#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Auth/IdentityWebAPI/IdentityWebAPI.csproj", "Auth/IdentityWebAPI/"]
COPY ["BuildingBlocks/Eventbus.Messages/Eventbus.Messages.csproj", "BuildingBlocks/Eventbus.Messages/"]
COPY ["BuildingBlocks/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "BuildingBlocks/EventBus.RabbitMQ/"]
RUN dotnet restore "./Auth/IdentityWebAPI/IdentityWebAPI.csproj"
COPY . .
WORKDIR "/src/Auth/IdentityWebAPI"
RUN dotnet build "./IdentityWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityWebAPI.dll"]