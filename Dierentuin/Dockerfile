# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dierentuin/Dierentuin.csproj", "Dierentuin/"]
COPY ["TestProject1/TestProject1.csproj", "TestProject1/"]
RUN dotnet restore "Dierentuin/Dierentuin.csproj"
RUN dotnet restore "TestProject1/TestProject1.csproj"
COPY . .
WORKDIR "/src/Dierentuin"
RUN dotnet build "Dierentuin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage for running tests
FROM build AS testrunner
WORKDIR /src/TestProject1
RUN dotnet test --logger:trx

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Dierentuin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#Install Goss for HealthCheck and Pipeline run
USER root
RUN apt update && apt install -y curl
RUN ARCH=$(uname -m) && \
    if [ "$ARCH" = "aarch64" ]; then \
      curl -L https://github.com/goss-org/goss/releases/download/v0.4.7/goss-linux-arm -o /usr/local/bin/goss; \
    elif [ "$ARCH" = "x86_64" ]; then \
      curl -L https://github.com/goss-org/goss/releases/download/v0.4.7/goss-linux-amd64 -o /usr/local/bin/goss; \
    else \
      echo "Unsupported architecture: $ARCH"; exit 1; \
    fi && \
    chmod +x /usr/local/bin/goss && \
    rm -rf /var/lib/apt/lists/* && \
    mkdir /goss && \
    ln -s /usr/local/bin/goss /goss/goss
USER app
#Voeg je Goss configuratiebestand toe aan de container
COPY /goss-healthcheck.yaml /goss.yaml
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 CMD goss validate --format documentation
ENTRYPOINT ["dotnet", "Dierentuin.dll"]