FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
# Adopted from https://docs.docker.com/engine/examples/dotnetcore/
# and from https://docs.microsoft.com/de-de/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-5.0

WORKDIR /app

COPY ./kata-rabbitmq.robot.app ./kata-rabbitmq.robot.app
COPY ./kata-rabbitmq.infrastructure ./kata-rabbitmq.infrastructure
COPY ./kata-rabbitmq.model ./kata-rabbitmq.model

RUN cd ./kata-rabbitmq.robot.app \
    && dotnet restore \
    && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/kata-rabbitmq.robot.app/out .
ENTRYPOINT ["dotnet", "kata-rabbitmq.robot.app.dll"]
