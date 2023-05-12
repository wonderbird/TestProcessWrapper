@PassCommandLineArgumentsToTestProcess
Feature: Pass Command Line Arguments to Test Process
  In order to control the behavior of the tested CLI application
  As a test developer
  I want to pass command line arguments to the wrapped test application.

Scenario: Specify command line argument with value
  Given An application was wrapped into TestProcessWrapper
  And the command line argument '--test-argument=test-argument-value' has been configured
  When the application is ready
  Then the application has received the command line argument '--test-argument' with value 'test-argument-value'
