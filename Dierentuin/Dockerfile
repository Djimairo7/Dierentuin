# Start from the ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
USER app

# Start the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dierentuin/Dierentuin.csproj", "Dierentuin/"]
COPY ["TestProject1/TestProject1.csproj", "TestProject1/"]
# Clear the NuGet cache
RUN dotnet nuget locals all --clear
# Restore the project dependencies with detailed logging
RUN dotnet restore "Dierentuin/Dierentuin.csproj" --verbosity detailed
RUN dotnet restore "TestProject1/TestProject1.csproj" --verbosity detailed
COPY . .
WORKDIR "/src/Dierentuin"
RUN dotnet build "Dierentuin.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage for running tests
FROM build AS testrunner
WORKDIR /src/TestProject1
RUN dotnet test --logger:trx

# Publish the application
FROM build AS publish
WORKDIR /src/Dierentuin
RUN dotnet publish "Dierentuin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Add debug information to check paths and contents
RUN ls -la /app/publish
RUN pwd

# Final stage / runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin.dll"]
