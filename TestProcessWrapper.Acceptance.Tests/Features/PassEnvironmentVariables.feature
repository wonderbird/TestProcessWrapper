@PassEnvironmentVariables
Feature: Pass Environment Variables
	In order to launch an application following the 12factor.net principles
  As a test developer using TestProcessWrapper
  I want to specify configuration parameters as environment variables

Scenario: Pass Environment Variables to the Process controlled by TestProcessWrapper
  Given A long lived application was wrapped into TestProcessWrapper
	And two environment variables have been configured
	When the application is ready
	Then the application has received the configured environment variables
