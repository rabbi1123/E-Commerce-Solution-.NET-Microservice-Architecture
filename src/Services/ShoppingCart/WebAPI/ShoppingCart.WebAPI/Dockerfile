#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ShoppingCart/WebAPI/ShoppingCart.WebAPI/ShoppingCart.WebAPI.csproj", "Services/ShoppingCart/WebAPI/ShoppingCart.WebAPI/"]
COPY ["Services/ShoppingCart/Core/ShoppingCart.Application/ShoppingCart.Application.csproj", "Services/ShoppingCart/Core/ShoppingCart.Application/"]
COPY ["BuildingBlocks/Eventbus.Messages/Eventbus.Messages.csproj", "BuildingBlocks/Eventbus.Messages/"]
COPY ["Services/ShoppingCart/Core/ShoppingCart.Domain/ShoppingCart.Domain.csproj", "Services/ShoppingCart/Core/ShoppingCart.Domain/"]
COPY ["Services/ShoppingCart/Infrastructure/ShoppingCart.Infrastructure/ShoppingCart.Infrastructure.csproj", "Services/ShoppingCart/Infrastructure/ShoppingCart.Infrastructure/"]
RUN dotnet restore "./Services/ShoppingCart/WebAPI/ShoppingCart.WebAPI/ShoppingCart.WebAPI.csproj"
COPY . .
WORKDIR "/src/Services/ShoppingCart/WebAPI/ShoppingCart.WebAPI"
RUN dotnet build "./ShoppingCart.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShoppingCart.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCart.WebAPI.dll"]