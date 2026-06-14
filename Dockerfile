FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY "backend/Resumi/Resumi.csproj" "backend/Resumi/"
RUN dotnet restore "backend/Resumi/Resumi.csproj"

COPY . .
WORKDIR "/src/backend/Resumi"
RUN dotnet build "Resumi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Resumi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Resumi.dll"]