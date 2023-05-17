@MatchBuildConfiguration
Feature: Consider build configuration when starting test process
	In order to run tests in both Release and Debug configuration
  As a test developer
  I want TestProcessWrapper to match the path of the test process to the build configuration of the test project

Scenario: Run tests in Release configuration
	Given A long lived application was wrapped into TestProcessWrapper
	And the build configuration 'Release' has been configured
	When the application is ready
	Then the application was launched from the 'Release' folder

Scenario: Run tests in Debug configuration
  Given A long lived application was wrapped into TestProcessWrapper
  And the build configuration 'Debug' has been configured
  When the application is ready
  Then the application was launched from the 'Debug' folder
