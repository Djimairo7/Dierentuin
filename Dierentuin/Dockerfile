# Base image from Microsoft & Expose ports
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Dierentuin/Dierentuin.csproj", "Dierentuin/"]
COPY ["TestProject1/TestProject1.csproj", "TestProject1/"]
RUN dotnet restore "Dierentuin/Dierentuin.csproj"
RUN dotnet restore "TestProject1/TestProject1.csproj"

COPY . .

WORKDIR /src/Dierentuin
RUN dotnet build "Dierentuin.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Dierentuin.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin.dll"]