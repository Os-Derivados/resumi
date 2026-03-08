FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release

WORKDIR /src

COPY ["backend/Resumi/Resumi.csproj", "backend/Resumi/"]
RUN dotnet restore "backend/Resumi/Resumi.csproj"

COPY . .
WORKDIR "/src/backend/Resumi"

RUN dotnet build "Resumi.csproj" -c $configuration -o /app/build

FROM build AS publish
RUN dotnet publish "Resumi.csproj" -c $configuration -o /app/publish \  
    /p:UseAppHost=false \
    /p:DebugType=portable \
    /p:DebugSymbols=true \
    /p:PublishReadyToRun=false \
    /p:PublishTrimmed=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Resumi.dll"]