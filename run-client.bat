@echo off
set DOTNET_ENVIRONMENT=Development

cd kata-rabbitmq.client.app\bin\Debug\net5.0
dotnet kata-rabbitmq.client.app.dll
cd ..\..\..\..
