@NoUnhandledExceptions
Feature: Coverlet Exception Handling
  In order to deliver a reliable product
  As a developer
  I do not want unforeseen exceptions to be reported

  Scenario: 1 application - no exceptions
    Given 1 application is running with coverlet 'enabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the log is free of exception messages

  Scenario: 2 applications - wrong usage - expected exceptions
    Given 2 applications are running with coverlet 'enabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And each log shows an exception message

  Scenario: 2 applications - correct usage - no exceptions
    Given 1 application is running with coverlet 'enabled'
    And 1 application is running with coverlet 'disabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the log is free of exception messages
    