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
dotnet build --configuration Debug --no-restore

# Test with coverage
dotnet test --no-restore --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='./TestResults/coverage.cobertura.xml'

# Build NuGet package
dotnet pack RemoteControlledProcess/RemoteControlledProcess.csproj "/p:PackageVersion=$VERSION"
