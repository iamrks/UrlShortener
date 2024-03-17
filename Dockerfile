FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UrlShortener.Api/UrlShortener.Api.csproj", "UrlShortener.Api/"]
RUN dotnet restore "UrlShortener.Api/UrlShortener.Api.csproj"
COPY . .
WORKDIR "/src/UrlShortener.Api"
RUN dotnet build "UrlShortener.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "UrlShortener.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet dev-certs https --trust

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for HTTPS
# ENV ASPNETCORE_URLS=https://+:443;http://+:8080

ENTRYPOINT ["dotnet", "UrlShortener.Api.dll"]