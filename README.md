# Test Helper: TestProcessWrapper

[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/wonderbird/TestProcessWrapper)
[![Build Status Badge](https://github.com/wonderbird/TestProcessWrapper/workflows/.NET/badge.svg)](https://github.com/wonderbird/TestProcessWrapper/actions)
[![Test Coverage (coveralls)](https://img.shields.io/coveralls/github/wonderbird/TestProcessWrapper)](https://coveralls.io/github/wonderbird/TestProcessWrapper)
[![Test Coverage (codeclimate)](https://img.shields.io/codeclimate/coverage-letter/wonderbird/TestProcessWrapper)](https://codeclimate.com/github/wonderbird/TestProcessWrapper/trends/test_coverage_total)
[![Code Maintainability](https://img.shields.io/codeclimate/maintainability-percentage/wonderbird/TestProcessWrapper)](https://codeclimate.com/github/wonderbird/TestProcessWrapper)
[![Issues in Code](https://img.shields.io/codeclimate/issues/wonderbird/TestProcessWrapper)](https://codeclimate.com/github/wonderbird/TestProcessWrapper/issues)
[![Technical Debt](https://img.shields.io/codeclimate/tech-debt/wonderbird/TestProcessWrapper)](https://codeclimate.com/github/wonderbird/TestProcessWrapper)
[![CodeScene Code Health](https://codescene.io/projects/12257/status-badges/code-health)](https://codescene.io/projects/12257/jobs/latest-successful/results)
[![CodeScene System Mastery](https://codescene.io/projects/12257/status-badges/system-mastery)](https://codescene.io/projects/12257/jobs/latest-successful/results)

<!-- doctoc --github --maxlevel 2 README.md -->
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Overview](#overview)
- [Attention](#attention)
- [Development and Support Standard](#development-and-support-standard)
- [Thanks](#thanks)
- [Development](#development)
- [Make a Release](#make-a-release)
- [References](#references)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Overview

Launch and control `dotnet` processes optionally wrapped into the
[coverlet](https://github.com/coverlet-coverage/coverlet) code coverage analyzer.

The class `TestProcessWrapper` is intended to launch one ore more `dotnet` processes for performing acceptance tests.
The class captures the messages written to the `Console` and to `Console.Error`. It allows to terminate the process
gracefully and forcefully. If multiple processes of the same executable (DLL) are running simultaneously, then one of
them can be wrapped by the [coverlet](https://github.com/coverlet-coverage/coverlet) command line tool in order to
calculate code coverage.

Example processes are given in this repository:

- [TestProcessWrapper.LongLived.Application](./TestProcessWrapper.LongLived.Application) - daemon process; terminates after receiving a signal
- [TestProcessWrapper.ShortLived.Application](./TestProcessWrapper.ShortLived.Application) - command line process; terminates quickly by itself

**Important**

- The tested process must report its process ID on the console. An example is
[TestProcessWrapper.ShortLived.Application/Program.cs](./TestProcessWrapper.ShortLived.Application/Program.cs).

- Ensure that **only one executable (DLL) is wrapped into 
  coverlet at a time**. Run instrumented integration tests sequentially.
  Otherwise the coverage results will be wrong. When using [specflow with xUnit](https://docs.specflow.org/projects/specflow/en/latest/Integrations/xUnit.html), enforce sequential execution by a
  [specflow.json file like this](https://github.com/wonderbird/malimo/blob/main/malimo.Acceptance.Tests/specflow.json).

**Usage Examples**

The most simple use is described by the acceptance test [SmokeTests.cs](TestProcessWrapper.Acceptance.Tests/Features/SmokeTests.cs).

You can find detailed usage examples in the [Acceptance Test Suite (BDD)](TestProcessWrapper.Acceptance.Tests). If you'd
like to have easy to read HTML documentation, then generate
[LivingDoc](https://docs.specflow.org/projects/getting-started/en/latest/gettingstartedrider/Step8r.html) as described
in the corresponding section [Create Feature Documentation (LivingDoc)](#create-feature-documentation-livingdoc) below.

Next, read a [Gherkin](https://specflow.org/learn/gherkin/) `.feature` file from the
[Features](TestProcessWrapper.Acceptance.Tests/Features) folder. It explains why each feature exists and which use
scenarios are addressed. Then read the corresponding `*StepDefinitions.cs` file in the
[Steps](TestProcessWrapper.Acceptance.Tests/Steps) folder. It shows how the test steps from the feature file (given,
when, then) are actually implemented. Note, that some frequently used steps are implemented in the
[Common](TestProcessWrapper.Acceptance.Tests/Steps/Common) folder.

Further information can be seen in the following GitHub repositories use TestProcessWrapper:
                            
- [wonderbird / malimo](https://github.com/wonderbird/malimo)
- [wonderbirds-katas / rabbitmq](https://github.com/wonderbirds-katas/kata-rabbitmq)

## Attention

You can use the `coverlet` wrapper only once per `dotnet` application, because `coverlet` instruments the `dotnet` DLL.
If you use `coverlet` with two or more instances of the same application, `coverlet` will report an exception after
(or during) the application termination and the reported coverage will be 0.

## Development and Support Standard

I am developing during my spare time and use this project for learning purposes. Please assume that I will need some
days to answer your questions. Please keep this in mind when using this project in a production environment.

## Thanks

Many thanks to [JetBrains](https://www.jetbrains.com/?from=TestProcessWrapper) who provide
an [Open Source License](https://www.jetbrains.com/community/opensource/) for this project ❤️.

## Development

### Prerequisites

To compile, test and run this project the [.NET SDK](https://dotnet.microsoft.com/download) is required on your machine.
The project supports .NET 6.0, .NET 7.0 and .NET 8.0.

For calculating code metrics I recommend [metrix++](https://github.com/metrixplusplus/metrixplusplus).
This requires [Python](https://www.python.org/).

If you'd like to contribute, then please use the [dotnet csharpier .](https://csharpier.com/) command as described
below.

To use the `TestProcessWrapper` library and to run the unit tests you need the following tools installed:

```shell
dotnet tool install --global coverlet.console --configfile NuGet-OfficialOnly.config
dotnet tool install --global dotnet-reportgenerator-globaltool --configfile NuGet-OfficialOnly.config
```

### Troubleshooting the Installation of dotnet tools

If you are installing a dotnet tool for the first time, then you'll need to add the path to the dotnet tools to your
system PATH. Please make sure that there is no "~" character in your PATH to coverlet.

E.g. add the following line to the end of your shell rc file (e.g. ~/.zshrc):

```shell
export PATH="$PATH:$HOME/.dotnet/tools"
```

### Build, Test, Run

Run the following commands from the folder containing the `TestProcessWrapper.sln` file in order to build, test and
run the application:

#### Build the Solution and Run the Acceptance Tests

Note: The script [build.sh](./build/build.sh) builds the NuGet package like the build pipeline does it. This can be helpful
when debugging issues popping up in the build pipeline.

Important: The acceptance tests require both a debug and a release build of the long lived application.

```sh

To build the solution and run the acceptance tests manually, issue the following console commands:

```sh
# Remove build output from previous runs
find . -iname "bin" -o -iname "obj" -exec rm -rf "{}" ";"

# The acceptance tests require both a debug and a release build of the long lived application
dotnet build --configuration Debug
dotnet build --configuration Release --no-restore TestProcessWrapper.LongLived.Application/TestProcessWrapper.LongLived.Application.csproj

# Simply run the tests
dotnet test

# As an alternative, run the tests with coverage and produce a coverage report
rm -r TestProcessWrapper.Acceptance.Tests/TestResults && \
  dotnet test --no-restore --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput='./TestResults/coverage.cobertura.xml' && \
  reportgenerator "-reports:TestProcessWrapper.Acceptance.Tests/TestResults/*.xml" "-targetdir:TestProcessWrapper.Acceptance.Tests/TestResults/report" "-reporttypes:Html;lcov" "-title:TestProcessWrapper"
open TestProcessWrapper.Acceptance.Tests/TestResults/report/index.html
```

#### Run the Smoke Tests

In the ci pipeline, the smoke tests verify compatibility with all supported .net framework versions.

To run the smoke tests locally, issue the following console commands:

```shell
# build the nuget package with version 0.0.0
dotnet pack --configuration Debug TestProcessWrapper/TestProcessWrapper.csproj

# run the smoke tests
cd TestProcessWrapper.Nupkg.Tests
./smoketest.sh "net8.0"
```

You can replace the "net8.0" with "net7.0" or "net6.0", if you want to test a different version of the .net framework.

#### Create Feature Documentation (LivingDoc)

As this project uses SpecFlow for acceptance tests, you can generate an HTML overview of all features including the test
status as follows.

Note: Despite the warnings, the commands produce correct documentation when using .NET 8.

```shell
# Prerequisite: Install the LivingDoc CLI once
dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI
```

```shell
# Run the acceptance tests to generate the TestExecution.json
# for LivingDoc
dotnet test TestProcessWrapper.Acceptance.Tests

# Generate LivingDoc
cd TestProcessWrapper.Acceptance.Tests/bin/Debug/net8.0
livingdoc test-assembly TestProcessWrapper.Acceptance.Tests.dll -t TestExecution.json
cd ../../../..

# Open the generated HTML in a browser
open TestProcessWrapper.Acceptance.Tests/bin/Debug/net8.0/LivingDoc.html
```

#### Before Creating a Pull Request ...

##### Fix Static Code Analysis Warnings

... fix static code analysis warnings reported by [SonarLint](https://www.sonarsource.com/products/sonarlint/)
and by [CodeClimate](https://codeclimate.com/github/wonderbird/TestProcessWrapper/issues).

##### Apply Code Formatting Rules

```shell
# Install https://csharpier.io globally, once
dotnet tool install -g csharpier

# Format code
dotnet csharpier .
```

##### Check Code Metrics

... check code metrics using [metrix++](https://github.com/metrixplusplus/metrixplusplus)

- Configure the location of the cloned metrix++ scripts
  ```shell
  export METRIXPP=/path/to/metrixplusplus
  ```

- Collect metrics
  ```shell
  python "$METRIXPP/metrix++.py" collect --std.code.complexity.cyclomatic --std.code.lines.code --std.code.todo.comments --std.code.maintindex.simple -- .
  ```

- Get an overview
  ```shell
  python "$METRIXPP/metrix++.py" view --db-file=./metrixpp.db
  ```

- Apply thresholds
  ```shell
  python "$METRIXPP/metrix++.py" limit --db-file=./metrixpp.db --max-limit=std.code.complexity:cyclomatic:5 --max-limit=std.code.lines:code:25:function --max-limit=std.code.todo:comments:0 --max-limit=std.code.mi:simple:1
  ```

At the time of writing, I want to stay below the following thresholds:

```text
--max-limit=std.code.complexity:cyclomatic:5
--max-limit=std.code.lines:code:25:function
--max-limit=std.code.todo:comments:0
--max-limit=std.code.mi:simple:1
```

Finally, remove all code duplication. The next section describes how to detect code duplication.

##### Remove Code Duplication Where Appropriate

To detect duplicates I use the [CPD Copy Paste Detector](https://docs.pmd-code.org/latest/pmd_userdocs_cpd.html)
tool from the [PMD Source Code Analyzer Project](https://docs.pmd-code.org/latest/index.html).

If you have installed PMD by download & unzip, replace `pmd` by `./run.sh`.
The [homebrew pmd formula](https://formulae.brew.sh/formula/pmd) makes the `pmd` command globally available.

```sh
# Remove temporary and generated files
# 1. dry run
git clean -ndx
```

```shell
# 2. Remove the files shown by the dry run
git clean -fdx
```

```shell
# Identify duplicated code in files to push to GitHub
pmd cpd --minimum-tokens 50 --language cs --dir .
```

## Make a Release

In order to create a release:

1. Create a branch to prepare the release
2. Update the [CHANGELOG.md](./CHANGELOG.md). The last number in the version is the build number. Assume that it will be
   the current build number + 2, because there will be one build to validate the pull request
3. Create a pull request and wait for the validations to complete
4. If there are validation errors, then fix them and update the release build number in the
   [CHANGELOG.md](./CHANGELOG.md)
5. The release will be published automatically from the main branch after the PR has been merged

## References

### .NET Core

* GitHub: [aspnet / Hosting / samples / GenericHostSample](https://github.com/aspnet/Hosting/tree/2.2.0/samples/GenericHostSample)

### Behavior Driven Development (BDD)

* Tricentis: [SpecFlow - Getting Started](https://specflow.org/getting-started/)
* The SpecFlow
  Team: [SpecFlow.xUnit — documentation](https://docs.specflow.org/projects/specflow/en/latest/Integrations/xUnit.html)
* The SpecFlow
  Team: [SpecFlow - Getting Started with a new project](https://docs.specflow.org/projects/specflow/en/latest/Getting-Started/Getting-Started-With-A-New-Project.html?utm_source=website&utm_medium=newproject&utm_campaign=getting_started)
* The SpecFlow Team: [Add Living Documentation](https://docs.specflow.org/projects/getting-started/en/latest/gettingstartedrider/Step8r.html)
* The SpecFlow Team: [Generating LivingDoc using CLI](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)
* [Testcontainers](https://www.testcontainers.org/)

### Code Style

* Bela VanderVoort: [CSharpier](https://csharpier.com/) - an opinionated code formatter
* Microsoft: [dotnet format](https://github.com/dotnet/format) - dotnet code formatter

### Code Analysis

* SonarSource S.A.: [SonarLint IDE Extension](https://www.sonarsource.com/products/sonarlint/)
* Microsoft: [Use code coverage for unit testing](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=linux)
* GitHub: [coverlet-coverage / coverlet](https://github.com/coverlet-coverage/coverlet)
* GitHub: [danielpalme / ReportGenerator](https://github.com/danielpalme/ReportGenerator)
* JetBrains s.r.o.: [dupFinder Command-Line Tool](https://www.jetbrains.com/help/resharper/dupFinder.html)
* Scott Hanselman: [EditorConfig code formatting from the command line with .NET Core's dotnet format global tool](https://www.hanselman.com/blog/editorconfig-code-formatting-from-the-command-line-with-net-cores-dotnet-format-global-tool)
* [EditorConfig.org](https://editorconfig.org)
* GitHub: [dotnet / roslyn - .editorconfig](https://github.com/dotnet/roslyn/blob/master/.editorconfig)
* Check all the badges on top of this README
