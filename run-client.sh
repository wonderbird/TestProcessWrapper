#!/bin/sh
pushd kata-rabbitmq.client.app/bin/Debug/net5.0/
DOTNET_ENVIRONMENT=Development dotnet kata-rabbitmq.client.app.dll
popd

