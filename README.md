# RabbitMQ Learning Kata

![Build Status Badge](https://github.com/wonderbird/kata-rabbitmq/workflows/.NET%20Core/badge.svg)
[![Test Coverage](https://img.shields.io/coveralls/github/wonderbird/kata-rabbitmq)](https://coveralls.io/github/wonderbird/kata-rabbitmq)
[![Code Maintainability](https://img.shields.io/codeclimate/maintainability-percentage/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)
[![Issues in Code](https://img.shields.io/codeclimate/issues/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq/issues)
[![Technical Debt](https://img.shields.io/codeclimate/tech-debt/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)

In this kata you learn how to use [RabbitMQ](https://www.rabbitmq.com).

**Attention**

This project is in an early stage. Please come back by mid November.

## Thanks

Many thanks to [JetBrains](https://www.jetbrains.com/?from=kata-rabbitmq) who provide an [Open Source License](https://www.jetbrains.com/community/opensource/) for this project ❤️.

# Required Steps

1. Launch an instance of RabbitMQ (e.g. https://hub.docker.com/_/rabbitmq)
2. Create a "robot" application which ...
   1. programmatically creates a message queue to transmit light sensor information (just some arbitrary values of ambient light)
   2. periodically sends light sensor information to the queue
3. Create a robot monitor application which consumes the message from the queue and displays them on the screen

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

# References

## RabbitMQ

* VMWare, Inc. or its affiliates: [RabbitMQ](https://www.rabbitmq.com/)
* VMWare, Inc. or its affiliates: [RabbitMQ .NET/C# Client API Guide](https://www.rabbitmq.com/dotnet-api-guide.html)
* [RabbitMQ .NET Client API Documentation](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.html)
* DockerHub: [RabbitMQ](https://hub.docker.com/_/rabbitmq)

## Behavior Driven Development (BDD)

* Tricentis: [SpecFlow - Getting Started](https://specflow.org/getting-started/)
* The SpecFlow Team: [SpecFlow.xUnit — documentation](https://docs.specflow.org/projects/specflow/en/latest/Integrations/xUnit.html)
* The SpecFlow Team: [SpecFlow - Getting Started with a new project](https://docs.specflow.org/projects/specflow/en/latest/Getting-Started/Getting-Started-With-A-New-Project.html?utm_source=website&utm_medium=newproject&utm_campaign=getting_started)

## Template For New Links

* [ ]( )
* [ ]( )
* [ ]( )
