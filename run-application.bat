@echo off
set DOTNET_ENVIRONMENT=Development

cd RemoteControlledProcess.Application\bin\Debug\net6.0
dotnet RemoteControlledProcess.Application.dll
cd ..\..\..\..
