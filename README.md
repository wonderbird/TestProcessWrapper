# RabbitMQ Learning Kata

[![Build Status Badge](https://github.com/wonderbird/kata-rabbitmq/workflows/.NET%20Core/badge.svg)](https://github.com/wonderbird/kata-rabbitmq/actions?query=workflow%3A%22.NET+Core%22)
[![Test Coverage](https://img.shields.io/coveralls/github/wonderbird/kata-rabbitmq)](https://coveralls.io/github/wonderbird/kata-rabbitmq)
[![Code Maintainability](https://img.shields.io/codeclimate/maintainability-percentage/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)
[![Issues in Code](https://img.shields.io/codeclimate/issues/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq/issues)
[![Technical Debt](https://img.shields.io/codeclimate/tech-debt/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq)

In this kata you learn how to use [RabbitMQ](https://www.rabbitmq.com).

**Attention**

This project is incomplete at the moment.

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

To build the project and run the acceptance tests

```sh
dotnet build
dotnet test
```

To run the application in a production environment

```sh
docker-compose build
docker-compose up
```

To run the application and RabbitMQ on your development PC

```sh
docker-compose rabbit up -d
cd kata-rabbitmq.robot.app/bin/Debug/net5.0
DOTNET_ENVIRONMENT=Development dotnet "kata-rabbitmq.robot.app"
docker-compose down
```

If you would like to run and debug the application in your IDE, make sure that
the environment variable `DOTNET_ENVIRONMENT` is set to `Development` so that
the application uses the RabbitMQ settings from `appsettings.Development.json`.

## Identify Code Duplication

The `tools\dupfinder.bat` or `tools/dupfinder.sh` file calls the [JetBrains dupfinder](https://www.jetbrains.com/help/resharper/dupFinder.html) tool and creates an HTML report of duplicated code blocks in the solution directory.

In order to use the `dupfinder` you need to globally install the [JetBrains ReSharper Command Line Tools](https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html)
On Unix like operating systems you also need [xsltproc](http://xmlsoft.org/XSLT/xsltproc2.html), which is pre-installed on macOS.

From the folder containing the `.sln` file run

```sh
tools\dupfinder.bat
```

or

```sh
tools/dupfinder.sh
```

respectively.

The report will be created as `dupfinder-report.html` in the current directory.

# References

## .NET Core

* GitHub: [aspnet / Hosting / samples / GenericHostSample](https://github.com/aspnet/Hosting/tree/2.2.0/samples/GenericHostSample)

## RabbitMQ

* VMWare, Inc. or its affiliates: [RabbitMQ](https://www.rabbitmq.com/)
* VMWare, Inc. or its affiliates: [RabbitMQ .NET/C# Client API Guide](https://www.rabbitmq.com/dotnet-api-guide.html)
* [RabbitMQ .NET Client API Documentation](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.html)
* DockerHub: [RabbitMQ](https://hub.docker.com/_/rabbitmq)

## Behavior Driven Development (BDD)

* Tricentis: [SpecFlow - Getting Started](https://specflow.org/getting-started/)
* The SpecFlow Team: [SpecFlow.xUnit — documentation](https://docs.specflow.org/projects/specflow/en/latest/Integrations/xUnit.html)
* The SpecFlow Team: [SpecFlow - Getting Started with a new project](https://docs.specflow.org/projects/specflow/en/latest/Getting-Started/Getting-Started-With-A-New-Project.html?utm_source=website&utm_medium=newproject&utm_campaign=getting_started)

## Code Analysis

* JetBrains s.r.o.: [dupFinder Command-Line Tool](https://www.jetbrains.com/help/resharper/dupFinder.html)
* GitHub: [coverlet-coverage / coverlet](https://github.com/coverlet-coverage/coverlet)
* GitHub: [danielpalme / ReportGenerator](https://github.com/danielpalme/ReportGenerator)
* Scott Hanselman: [EditorConfig code formatting from the command line with .NET Core's dotnet format global tool](https://www.hanselman.com/blog/editorconfig-code-formatting-from-the-command-line-with-net-cores-dotnet-format-global-tool)
* [EditorConfig.org](https://editorconfig.org)
* GitHub: [dotnet / roslyn - .editorconfig](https://github.com/dotnet/roslyn/blob/master/.editorconfig)

## Template For New Links

* [ ]( )
* [ ]( )
