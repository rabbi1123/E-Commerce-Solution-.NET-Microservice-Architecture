#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ProductCatalog/WebAPI/ECommerce.WebAPI/ECommerce.WebAPI.csproj", "Services/ProductCatalog/WebAPI/ECommerce.WebAPI/"]
COPY ["Services/ProductCatalog/Core/ECommerce.Application/ECommerce.Application.csproj", "Services/ProductCatalog/Core/ECommerce.Application/"]
COPY ["Services/ProductCatalog/Core/ECommerce.Domain/ECommerce.Domain.csproj", "Services/ProductCatalog/Core/ECommerce.Domain/"]
COPY ["Services/ProductCatalog/Infrastructure/ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "Services/ProductCatalog/Infrastructure/ECommerce.Infrastructure/"]
RUN dotnet restore "./Services/ProductCatalog/WebAPI/ECommerce.WebAPI/ECommerce.WebAPI.csproj"
COPY . .
WORKDIR "/src/Services/ProductCatalog/WebAPI/ECommerce.WebAPI"
RUN dotnet build "./ECommerce.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ECommerce.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.WebAPI.dll"]