Feature: Short Lived Application
	In order to test command line interface applications (CLI)
  As a test developer
  I want to test an applications terminating on its own after its task is complete.

Scenario: Application shuts down after printing its process id
	Given 1 'short' lived application is running with coverlet 'enabled'
  When all applications had enough time to finish
  Then all applications shut down
	And the reported total line coverage is greater 0%
