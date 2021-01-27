@NoUnhandledExceptions
Feature: No Unhandeled Exceptions
  In order to deliver a reliable product
  As a developer
  I do not want exceptions to be reported

  Scenario: 1 application - unhandeled exception regression test
    Given 1 application is running
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the log is free of exception messages

  Scenario: 2 applications - unhandeled exception regression test
    Given 2 applications are running
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the log is free of exception messages
    