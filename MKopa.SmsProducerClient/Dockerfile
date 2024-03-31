#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MKopa.SmsProducerClient/MKopa.SmsProducerClient.csproj", "MKopa.SmsProducerClient/"]
COPY ["MKopa.Common/MKopa.Core.csproj", "MKopa.Common/"]
COPY ["MKopa.DataAccess/MKopa.DataAccess.csproj", "MKopa.DataAccess/"]
RUN dotnet restore "./MKopa.SmsProducerClient/MKopa.SmsProducerClient.csproj"
COPY . .
WORKDIR "/src/MKopa.SmsProducerClient"
RUN dotnet build "./MKopa.SmsProducerClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MKopa.SmsProducerClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MKopa.SmsProducerClient.dll"]