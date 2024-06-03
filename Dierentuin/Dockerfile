# Basis image voor runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image voor het bouwen en testen van de applicatie
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Dierentuin/Dierentuin.csproj", "Dierentuin/"]
COPY ["TestProject1/TestProject1.csproj", "TestProject1/"]
RUN dotnet restore "Dierentuin/Dierentuin.csproj"
RUN dotnet restore "TestProject1/TestProject1.csproj"
COPY . .
WORKDIR "/src/Dierentuin"
RUN dotnet build "Dierentuin.csproj" -c Release -o /app/build

# Aparte stage voor het uitvoeren van tests
FROM build AS testrunner
WORKDIR /src/TestProject1
RUN dotnet test --logger:trx

# Stage voor het publiceren van de applicatie
FROM build AS publish
RUN dotnet publish "Dierentuin/Dierentuin.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage voor het draaien van de applicatie
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin.dll"]
