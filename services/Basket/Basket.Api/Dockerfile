# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/Basket/Basket.Api/Basket.Api.csproj", "services/Basket/Basket.Api/"]
COPY ["services/Basket/Basket.Application/Basket.Application.csproj", "services/Basket/Basket.Application/"]
COPY ["services/Basket/Basket.Core/Basket.Core.csproj", "services/Basket/Basket.Core/"]
COPY ["services/Basket/Basket.Infrastructure/Basket.Infrastructure.csproj", "services/Basket/Basket.Infrastructure/"]
COPY ["Infrastructure/EventBus.Messages/EventBus.Messages.csproj" , "Infrastructure/EventBus.Messages/"]
COPY ["Infrastructure/common.logging/common.logging.csproj" , "Infrastructure/common.logging"]
RUN dotnet restore "./services/Basket/Basket.Api/Basket.Api.csproj"
COPY . .
WORKDIR "/src/services/Basket/Basket.Api"
RUN dotnet build "./Basket.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Basket.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]