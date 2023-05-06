#!/bin/sh

pushd RemoteControlledProcess.LongLived.Application/bin/Debug/net7.0
DOTNET_ENVIRONMENT=Development dotnet RemoteControlledProcess.LongLived.Application.dll
popd

