# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# copy solution and project files and restore as distinct layers
COPY backend/Resumi.sln .
# copy project source files (copy full project folders before restore to ensure analyzers and assets are available)
COPY backend/Resumi/. ./Resumi/
COPY backend/TestResumi/. ./TestResumi/
RUN dotnet restore

WORKDIR /source/Resumi
RUN dotnet publish -c Release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Resumi.dll"]