#!/bin/sh

pushd RemoteControlledProcess.Application/bin/Debug/net7.0
DOTNET_ENVIRONMENT=Development dotnet RemoteControlledProcess.Application.dll
popd

