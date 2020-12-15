#!/bin/sh
pushd kata-rabbitmq.robot.app/bin/Debug/net5.0/
DOTNET_ENVIRONMENT=Development dotnet kata-rabbitmq.robot.app.dll
popd

