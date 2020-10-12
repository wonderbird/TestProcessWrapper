# RabbitMQ Learning Kata

![Build Status Badge](https://github.com/wonderbird/kata-rabbitmq/workflows/.NET%20Core/badge.svg)
[![Test Coverage](https://img.shields.io/codeclimate/coverage/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)
[![Code Maintainability](https://img.shields.io/codeclimate/maintainability-percentage/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)
[![Issues in Code](https://img.shields.io/codeclimate/issues/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq/issues)
[![Technical Debt](https://img.shields.io/codeclimate/tech-debt/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)

In this kata you learn how to setup and use [RabbitMQ](https://www.rabbitmq.com).

**Attention**

This project is in an early stage. Please come back by mid November.

# Development

## Prerequisites

To compile, test and run this project the latest [.NET Core SDK](https://dotnet.microsoft.com/download) is required on your machine.

## Build, Test, Run

On any computer with the [.NET Core SDK](https://dotnet.microsoft.com/download) run the following commands from the folder containing the `kata-rabbitmq.sln` file in order to build, test and run the application:

```sh
dotnet build
dotnet test
dotnet run --project "kata-rabbitmq.App"
```

## Identify Code Duplication (Windows only)

The `tools\dupfinder.bat` file calls the [JetBrains dupfinder](https://www.jetbrains.com/help/resharper/dupFinder.html) tool and creates an HTML report of duplicated code blocks in the solution directory.

In order to use the `tools\dupfinder.bat` you need to globally install the [JetBrains ReSharper Command Line Tools](https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html)

From the folder containing the `.sln` file run

```
tools\dupfinder.bat
```

The report will be created as `dupfinder-report.html` in the current directory.
