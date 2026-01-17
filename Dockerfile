# https://hub.docker.com/_/microsoft-dotnet
# build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# copy solution and project files and restore as distinct layers
COPY backend/Resumi.sln .
# copy project source files (copy full project folders before restore to ensure analyzers and assets are available)
COPY backend/Resumi/. ./Resumi/
COPY backend/TestResumi/. ./TestResumi/
RUN dotnet restore

ARG BUILD_CONFIGURATION

WORKDIR /source/Resumi
RUN dotnet publish -c ${BUILD_CONFIGURATION} -o /app --no-restore

# final runtime image (smaller) for CI / production
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final-runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Resumi.dll"]

# final debug image (uses SDK so the container can be debugged / have vsdbg injected)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final-debug
WORKDIR /app
COPY --from=build /app ./
# Keep container alive without starting the app - debugger will start it
ENTRYPOINT ["tail", "-f", "/dev/null"]