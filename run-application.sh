#!/bin/sh

pushd TestProcessWrapper.LongLived.Application/bin/Debug/net7.0
DOTNET_ENVIRONMENT=Development dotnet TestProcessWrapper.LongLived.Application.dll
popd

