FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
LABEL vendor="Tributech.io Solutions"
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
WORKDIR /src
COPY ["src/Tributech.DataSpace.TwinAPI/Tributech.DataSpace.TwinAPI.csproj", "src/Tributech.DataSpace.TwinAPI/"]
RUN dotnet restore "src/Tributech.DataSpace.TwinAPI/Tributech.DataSpace.TwinAPI.csproj"
COPY . .
WORKDIR "/src/src/Tributech.DataSpace.TwinAPI"
RUN dotnet build "Tributech.DataSpace.TwinAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tributech.DataSpace.TwinAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
HEALTHCHECK --interval=30s --timeout=30s CMD curl -L --silent --fail --show-error http://localhost:80/health || exit 1
# install curl for health-check
# see in sdk: https://github.com/dotnet/dotnet-docker/blob/0c3cedc7cc5c6679edbba3a0fdf717caeefe02bb/src/sdk/5.0/buster-slim/amd64/Dockerfile#L15
RUN apt-get update \
	&& apt-get install -y --no-install-recommends curl \
	&& rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tributech.DataSpace.TwinAPI.dll"]