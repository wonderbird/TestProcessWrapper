FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
# Adopted from https://docs.docker.com/engine/examples/dotnetcore/

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "kata-rabbitmq.robot.app.dll"]
