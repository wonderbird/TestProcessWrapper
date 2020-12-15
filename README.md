# RabbitMQ Learning Kata

[![Build Status Badge](https://github.com/wonderbird/kata-rabbitmq/workflows/.NET%20Core/badge.svg)](https://github.com/wonderbird/kata-rabbitmq/actions?query=workflow%3A%22.NET+Core%22)
[![Test Coverage (coveralls)](https://img.shields.io/coveralls/github/wonderbird/kata-rabbitmq)](https://coveralls.io/github/wonderbird/kata-rabbitmq)
[![Test Coverage (codeclimate)](https://img.shields.io/codeclimate/coverage-letter/wonderbird/kata-rabbitmq)](https://codeclimate.com/github/wonderbird/kata-rabbitmq/trends/test_coverage_total)
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
   2. periodically sends arbitrary light sensor information to the queue (e.g. the JSON string '{ "sensor1": "7.0" }')
3. Create a robot monitor application which ...
   1. consumes the message from the queue and displays them on the screen
   2. acknowledges the messages it consumed
 

999. Further Ideas: Consider the information in section "Important Production Related Documentation" below

# Development

## Prerequisites

To compile, test and run this project the latest [.NET Core SDK](https://dotnet.microsoft.com/download) is required on your machine.

## Build, Test, Run

On any computer with the [.NET Core SDK](https://dotnet.microsoft.com/download) run the following commands from the folder containing the `kata-rabbitmq.sln` file in order to build, test and run the application:

### Build the Project and Run the Acceptance Tests

```sh
dotnet build
dotnet test
```

### Run the Application in a Production Environment

```sh
docker-compose build
docker-compose up
```

### Run the Application and RabbitMQ on Your Development PC

```sh
# Launch the RabbitMQ container
docker-compose rabbit up -d
# Optional: attach to the container logs
docker-compose logs -f

# Run the client
./run-client.sh

# Run the robot (server)
./run-robot.sh

# Hit CTL-C in both applications to shut them down

# Cleanup the docker containers
docker-compose down --remove-orphans
```

If you would like to run and debug the application in your IDE, make sure that
the environment variable `DOTNET_ENVIRONMENT` is set to `Development` so that
the application uses the logging and RabbitMQ settings from
`appsettings.Development.json`.

### Debug Acceptance Tests

The acceptance tests use [Testcontainers](https://www.testcontainers.org/) to
start and tear down RabbitMq. For debugging this means that on every test run
a container would be started. This leads to waiting times of about 20 seconds
per test run.

To avoid this delay, you can run a RabbitMq container via

```sh
docker-compose up -d rabbit
```

Then comment out the `[Binding]` attribute in class `SetupAndTearDownRabbitMq`
and uncomment the `[Binding]` attribute in class `SetupAndTearDownRabbitMqWithoutTestcontainer`.

**Attention**

Please never checkin these comment changes in `SetupAndTearDownRabbitMq*`. Otherwise
the automatic build will fail.

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

### General RabbitMQ Documentation

* VMWare, Inc. or its affiliates: [RabbitMQ](https://www.rabbitmq.com/)
* VMWare, Inc. or its affiliates: [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)

### Programming Related Documentation

* VMWare, Inc. or its affiliates: [RabbitMQ .NET/C# Client API Guide](https://www.rabbitmq.com/dotnet-api-guide.html)
* [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
* GitHub: [rabbitmq / rabbitmq-tutorials](https://github.com/rabbitmq/rabbitmq-tutorials)
* [RabbitMQ .NET Client API Documentation](http://rabbitmq.github.io/rabbitmq-dotnet-client/api/RabbitMQ.Client.html)
* GitHub: [rabbitmq / rabbitmq-dotnet-client](https://github.com/rabbitmq/rabbitmq-dotnet-client)
* DockerHub: [RabbitMQ](https://hub.docker.com/_/rabbitmq)

### Important Production Related Documentation

* VMWare, Inc. or its affiliates: [RabbitMQ Consumer Acknowledgements and Publisher Confirms](https://www.rabbitmq.com/confirms.html)
* VMWare, Inc. or its affiliates: [RabbitMQ Production Checklist](https://www.rabbitmq.com/production-checklist.html)
* VMWare, Inc. or its affiliates: [RabbitMQ Monitoring](https://www.rabbitmq.com/monitoring.html)

## Behavior Driven Development (BDD)

* Tricentis: [SpecFlow - Getting Started](https://specflow.org/getting-started/)
* The SpecFlow Team: [SpecFlow.xUnit — documentation](https://docs.specflow.org/projects/specflow/en/latest/Integrations/xUnit.html)
* The SpecFlow Team: [SpecFlow - Getting Started with a new project](https://docs.specflow.org/projects/specflow/en/latest/Getting-Started/Getting-Started-With-A-New-Project.html?utm_source=website&utm_medium=newproject&utm_campaign=getting_started)
* [Testcontainers](https://www.testcontainers.org/)

## Code Analysis

* JetBrains s.r.o.: [dupFinder Command-Line Tool](https://www.jetbrains.com/help/resharper/dupFinder.html)
* GitHub: [coverlet-coverage / coverlet](https://github.com/coverlet-coverage/coverlet)
* GitHub: [danielpalme / ReportGenerator](https://github.com/danielpalme/ReportGenerator)
* Scott Hanselman: [EditorConfig code formatting from the command line with .NET Core's dotnet format global tool](https://www.hanselman.com/blog/editorconfig-code-formatting-from-the-command-line-with-net-cores-dotnet-format-global-tool)
* [EditorConfig.org](https://editorconfig.org)
* GitHub: [dotnet / roslyn - .editorconfig](https://github.com/dotnet/roslyn/blob/master/.editorconfig)
* Check all the badges on top of this README

## Template For New Links

* [ ]( )
* [ ]( )
