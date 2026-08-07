# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app-build
COPY ["src/ArticlesAPI/ArticlesAPI.csproj", "src/ArticlesAPI/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Cache/ArticlesApp.Infrastructure.Cache.csproj", "src/Infrastructure/Cache/"]
COPY ["src/Infrastructure/Common/ArticlesApp.Infrastructure.Common.csproj", "src/Infrastructure/Common/"]
COPY ["src/Infrastructure/DataAccess/ArticlesApp.Infrastructure.DataAccess.csproj", "src/Infrastructure/DataAccess/"]
COPY ["src/Infrastructure/Logging/ArticlesApp.Infrastructure.Logging.csproj", "src/Infrastructure/Logging/"]


RUN dotnet restore "src/ArticlesAPI/ArticlesAPI.csproj"
COPY . .
#WORKDIR "src/ArticlesAPI"
RUN dotnet build "src/ArticlesAPI/ArticlesAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Test stage
FROM build AS test
RUN dotnet test --no-build --configuration Release

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/ArticlesAPI/ArticlesAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArticlesAPI.dll"]