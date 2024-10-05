#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SatisfactoryBot/SatisfactoryBot.csproj", "SatisfactoryBot/"]
COPY ["SatisfactoryBot.Data/SatisfactoryBot.Data.csproj", "SatisfactoryBot.Data/"]
RUN dotnet restore "./SatisfactoryBot/SatisfactoryBot.csproj"
COPY . .
WORKDIR "/src/SatisfactoryBot"
RUN dotnet build "./SatisfactoryBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SatisfactoryBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SatisfactoryBot.dll"]



##DockerFile ARM64

#FROM mcr.microsoft.com/dotnet/runtime:8.0-bookworm-slim-arm64v8 AS base
#USER app
#WORKDIR /app
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim-arm64v8 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["SatisfactoryBot/SatisfactoryBot.csproj", "SatisfactoryBot/"]
#COPY ["SatisfactoryBot.Data/SatisfactoryBot.Data.csproj", "SatisfactoryBot.Data/"]
#RUN dotnet restore "./SatisfactoryBot/SatisfactoryBot.csproj"
#COPY . .
#WORKDIR "/src/SatisfactoryBot"
#RUN dotnet build "./SatisfactoryBot.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./SatisfactoryBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -r linux-arm64
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SatisfactoryBot.dll"]