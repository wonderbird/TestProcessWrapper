FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
# Adopted from https://docs.docker.com/engine/examples/dotnetcore/

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "kata-rabbitmq.robot.app.dll"]
