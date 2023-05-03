Feature: Short Lived Application
	In order to test command line interface applications (CLI)
  As a test developer
  I want to test an applications terminating after its task is complete.

Scenario: Application shuts down after printing its process id
	Given 1 application is running with coverlet 'enabled'
  When all applications are shut down gracefully
	Then the reported total line coverage is greater 0%
