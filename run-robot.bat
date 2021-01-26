@echo off
set DOTNET_ENVIRONMENT=Development

cd kata-rabbitmq.robot.app\bin\Debug\net5.0
dotnet kata-rabbitmq.robot.app.dll
cd ..\..\..\..

