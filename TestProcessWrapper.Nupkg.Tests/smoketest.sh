#!/bin/sh
#
# Smoke test: Does the created NuGet Package work?
#
set -euf

#####
# Arrange
#####

echo "***** Cleaning up previous test run"
rm -vrf ./SmokeTest.Test
rm -vrf ./TestProcessWrapper.ShortLived.Application
rm -vf ./SmokeTest.sln
rm -vf ./global.json
echo

echo "***** Clear local nuget cache"
rm -vf NuGet.config
dotnet nuget locals all --clear
echo

echo "***** Configure .NET framework version: net6.0"
export framework_version="net6.0"
cp -v "${framework_version}-global.json" global.json
echo

echo "***** Building test application used by smoke test"
cp -vR "../TestProcessWrapper.ShortLived.Application" "TestProcessWrapper.ShortLived.Application"
rm -vfr "TestProcessWrapper.ShortLived.Application/bin"
rm -vfr "TestProcessWrapper.ShortLived.Application/obj"

if [ "$(uname)" = "Darwin" ]; then
  # on macOS, the sed -i parameter requires empty single quotes
  sed -i '' "s/net7.0/${framework_version}/" "TestProcessWrapper.ShortLived.Application/TestProcessWrapper.ShortLived.Application.csproj"
else
  sed -i "s/net7.0/${framework_version}/" "TestProcessWrapper.ShortLived.Application/TestProcessWrapper.ShortLived.Application.csproj"
fi

dotnet build --configuration Debug "TestProcessWrapper.ShortLived.Application/TestProcessWrapper.ShortLived.Application.csproj"
echo

echo "***** Preparing new dotnet solution with an xunit test project"
dotnet new sln --name SmokeTest
dotnet new xunit --name SmokeTest.Test
dotnet sln add SmokeTest.Test/SmokeTest.Test.csproj

# We need to add the dependencies of our own NuGet package before switching to the local package directory
dotnet add SmokeTest.Test/SmokeTest.Test.csproj package Microsoft.Extensions.Logging.Abstractions
dotnet add SmokeTest.Test/SmokeTest.Test.csproj package xunit.abstractions
echo

echo "***** Coping smoke test which uses TestProcessWrapper"
cp -v ../TestProcessWrapper.Acceptance.Tests/Features/SmokeTests.cs SmokeTest.Test/UnitTest1.cs
echo

echo "***** Configure NuGet local package directory"
cp -v NuGet-Local.config NuGet.config
echo

#####
# Act
#####

echo "***** Installing NuGet package provided by this solution"
dotnet add SmokeTest.Test/SmokeTest.Test.csproj package Boos.TestProcessWrapper --prerelease
echo

echo "***** Executing smoke test"
dotnet test

# Save the exit value of the test process
SUCCESS=$?
echo

#####
# Assert
#####

echo "***** Ensuring smoke test was successful"
if [ $SUCCESS -ne 0 ]; then
  echo "FAILED: SmokeTest failed."
  echo
  echo "Please check the messages above. Has the nuget package been created correctly"
  echo "in the folder referenced by NuGet.config in the current directory?"
  exit 1
fi

echo "***** Removing overwritten NuGet.config"
rm -v NuGet.config
