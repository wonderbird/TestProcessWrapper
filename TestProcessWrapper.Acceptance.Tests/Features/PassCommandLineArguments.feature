@PassCommandLineArgumentsToTestProcess
Feature: Pass Command Line Arguments to Test Process
  In order to control the behavior of the tested CLI application
  As a test developer
  I want to pass command line arguments to the wrapped test application.

Scenario: Specify boolean flag as command line argument
  Given An application was wrapped into TestProcessWrapper
  And the command line argument '--help=true' has been configured
  When the application is ready
  Then the application has received the command line argument
