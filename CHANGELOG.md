# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Every release is published as nuget package [Boos.TestProcessWrapper](https://www.nuget.org/packages/Boos.TestProcessWrapper/).

## [6.0.317-alpha] - 2023-05-17

### Added (Breaking Change)

- The build configuration of the process under test can be selected (Debug or Release). This is a **breaking change**, because the `TestProcessWrapper` constructor forces you to specify a `BuildConfiguration`. This shall ensure that the selected build configuration matches your CI/CD pipeline configuration.

### Changed

- Upgrade dependencies: Microsoft.NET.Test.Sdk 17.6.0

## [5.0.306-alpha] - 2023-05-15

### Added

- A boolean option "--option" can be passed to the application under test as command line argument
- A "--key=value" pair can be passed to the application under test as command line argument

## [5.0.300-alpha] - 2023-05-10

### Added

- README.md shows how to make a release

## [5.0.298-alpha] - 2023-05-09

### Changed

- automatically create GitHub release
- improve code style
