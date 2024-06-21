FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EndpointAnalyzer/EndpointAnalyzer.csproj", "EndpointAnalyzer/"]
RUN dotnet restore "EndpointAnalyzer/EndpointAnalyzer.csproj"
COPY . .
WORKDIR "/src/EndpointAnalyzer"
RUN dotnet build "EndpointAnalyzer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EndpointAnalyzer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EndpointAnalyzer.dll"]
