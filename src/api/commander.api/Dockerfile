FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props Directory.Build.props ./
COPY src/api/commander.api/commander.api.csproj src/api/commander.api/
COPY src/api/commander.application/commander.application.csproj src/api/commander.application/
COPY src/api/commander.domain/commander.domain.csproj src/api/commander.domain/
COPY src/api/commander.infrastructure/commander.infrastructure.csproj src/api/commander.infrastructure/

RUN dotnet restore src/api/commander.api/commander.api.csproj

COPY src/api/ src/api/

RUN dotnet publish src/api/commander.api/commander.api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "commander.api.dll"]