#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Ordering/WebAPI/Order.API/Order.API.csproj", "Services/Ordering/WebAPI/Order.API/"]
COPY ["BuildingBlocks/Eventbus.Messages/Eventbus.Messages.csproj", "BuildingBlocks/Eventbus.Messages/"]
COPY ["BuildingBlocks/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "BuildingBlocks/EventBus.RabbitMQ/"]
COPY ["Services/Ordering/Core/Order.Application/Order.Application.csproj", "Services/Ordering/Core/Order.Application/"]
COPY ["Services/Ordering/Core/Order.Domain/Order.Domain.csproj", "Services/Ordering/Core/Order.Domain/"]
COPY ["Services/Ordering/Infrastructure/Order.Infrastructure/Order.Infrastructure.csproj", "Services/Ordering/Infrastructure/Order.Infrastructure/"]
RUN dotnet restore "./Services/Ordering/WebAPI/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/Services/Ordering/WebAPI/Order.API"
RUN dotnet build "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]