@echo off
set DOTNET_ENVIRONMENT=Development

cd RemoteControlledProcess.LongLived.Application\bin\Debug\net7.0
dotnet RemoteControlledProcess.LongLived.Application.dll
cd ..\..\..\..
