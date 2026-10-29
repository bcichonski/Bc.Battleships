# syntax=docker/dockerfile:1

# Stage 1: build the SPA
FROM node:24 AS client
WORKDIR /src/src/Bc.Battleships/ClientApp
COPY src/Bc.Battleships/ClientApp/package.json src/Bc.Battleships/ClientApp/package-lock.json ./
RUN npm ci
COPY src/Bc.Battleships/ClientApp/ ./
RUN npm run build

# Stage 2: publish the web host (SPA already built, npm skipped)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS publish
WORKDIR /src
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/Bc.Battleships/Bc.Battleships.csproj src/Bc.Battleships/
COPY src/Bc.Battleships.Application/Bc.Battleships.Application.csproj src/Bc.Battleships.Application/
COPY src/Bc.Battleships.Domain/Bc.Battleships.Domain.csproj src/Bc.Battleships.Domain/
COPY src/Bc.Battleships.Infrastructure/Bc.Battleships.Infrastructure.csproj src/Bc.Battleships.Infrastructure/
RUN dotnet restore src/Bc.Battleships/Bc.Battleships.csproj
COPY src/ src/
COPY --from=client /src/src/Bc.Battleships/ClientApp/build/ src/Bc.Battleships/ClientApp/build/
RUN dotnet publish src/Bc.Battleships/Bc.Battleships.csproj -c Release --no-restore -p:SkipClientBuild=true -o /app/publish

# Stage 3: runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
RUN mkdir -p /data && chown 1654:1654 /data
WORKDIR /app
COPY --from=publish /app/publish ./
ENV ConnectionStrings__ConnectionString="Data Source=/data/battleships.db"
EXPOSE 8080
USER app
HEALTHCHECK --interval=10s --timeout=3s --start-period=10s --retries=3 \
    CMD curl --fail --silent http://localhost:8080/healthz || exit 1
ENTRYPOINT ["dotnet", "Bc.Battleships.dll"]
