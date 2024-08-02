#!/bin/sh
#
# Build and test the nuget package
#
# USAGE:
# build.sh VERSION
#
# VERSION  specifies the version number of the nuget package.
#          If not present, the default is "0.0.0-alpha".
#          The suffix "-alpha" may only be removed, if the package
#          is built by the build pipeline for the main branch.
#
set -euxf

if [ "$#" -ne 1 ]; then
  VERSION="0.0.0-alpha"
else
  VERSION="$1"
fi

# Install dependencies
dotnet restore

# Build
# Note: The acceptance tests require both a debug and a release build of the long lived application
dotnet build --configuration Debug --no-restore
dotnet build --configuration Release --no-restore TestProcessWrapper.LongLived.Application/TestProcessWrapper.LongLived.Application.csproj

# Test with coverage
dotnet test --no-restore --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='./TestResults/coverage.cobertura.xml'

# Build NuGet package
dotnet pack TestProcessWrapper/TestProcessWrapper.csproj "/p:PackageVersion=$VERSION"
