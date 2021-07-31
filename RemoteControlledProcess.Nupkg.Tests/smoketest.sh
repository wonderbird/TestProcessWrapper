#!/bin/sh
#
# Smoke test: Does the created NuGet Package work?
#

#####
# Arrange
#####

echo "***** Cleaning up previous test run"
rm -vrf ./SmokeTest.Test
rm -vrf ./RemoteControlledProcess.Application
rm -vf ./SmokeTest.sln
echo

echo "***** Clear local nuget cache"
rm -v NuGet.config
dotnet nuget locals all --clear
echo

echo "***** Copying test application used by smoke test"
mkdir -p RemoteControlledProcess.Application/bin/Debug/net5.0
cp -vR ../RemoteControlledProcess.Application/bin/Debug/net5.0/* RemoteControlledProcess.Application/bin/Debug/net5.0
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
cp -v ../RemoteControlledProcess.Acceptance.Tests/Features/SmokeTests.cs SmokeTest.Test/UnitTest1.cs
echo

echo "***** Configure NuGet local package directory"
cp -v NuGet-Local.config NuGet.config
echo

#####
# Act
#####

echo "***** Installing NuGet package provided by this solution"
dotnet add SmokeTest.Test/SmokeTest.Test.csproj package Boos.RemoteControlledProcess --prerelease
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
