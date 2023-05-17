@SelectBuildConfiguration
Feature: Select build configuration for test process
	In order to run tests in either Release or Debug configuration
  As a test developer
  I want to tell TestProcessWrapper which build configuration to use for the process under test

Scenario: Run process under test in 'Release' configuration
	Given A long lived application was wrapped into TestProcessWrapper
	And the build configuration 'Release' has been selected
	When the application is ready
	Then the application was launched from the 'Release' folder

Scenario: Run process under test in 'Debug' configuration
  Given A long lived application was wrapped into TestProcessWrapper
  And the build configuration 'Debug' has been selected
  When the application is ready
  Then the application was launched from the 'Debug' folder
