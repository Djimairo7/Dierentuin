FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Dierentuin/Dierentuin.csproj", "Dierentuin/"]
COPY ["TestProject1/TestProject1.csproj", "TestProject1/"]
RUN dotnet restore "Dierentuin/Dierentuin.csproj"
RUN dotnet restore "TestProject1/TestProject1.csproj"
COPY . .
WORKDIR "/src/Dierentuin"
RUN dotnet build "Dierentuin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dierentuin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dierentuin.dll"]
