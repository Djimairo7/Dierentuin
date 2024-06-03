# Start from the ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Make sure the user 'app' exists
RUN groupadd -r app && useradd -r -g app app
USER app

# Start the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dierentuin/Dierentuin.csproj", "Dierentuin/"]
COPY ["TestProject1/TestProject1.csproj", "TestProject1/"]
# Restore the project dependencies
RUN dotnet restore "Dierentuin/Dierentuin.csproj"
RUN dotnet restore "TestProject1/TestProject1.csproj"
COPY . .
WORKDIR "/src/Dierentuin"
RUN dotnet build "Dierentuin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage for running tests
FROM build AS testrunner
WORKDIR /src/TestProject1
RUN dotnet test --logger:trx

# Publish the application
FROM build AS publish
RUN dotnet publish "Dierentuin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage / runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin.dll"]
