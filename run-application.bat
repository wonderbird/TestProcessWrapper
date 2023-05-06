@echo off
set DOTNET_ENVIRONMENT=Development

cd TestProcessWrapper.LongLived.Application\bin\Debug\net7.0
dotnet TestProcessWrapper.LongLived.Application.dll
cd ..\..\..\..
