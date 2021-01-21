@NoUnhandledExceptions
Feature: No Unhandeled Exceptions
  In order to deliver a reliable product
  As a developer
  I do not want exceptions to be reported

  Scenario: Unhandeled Exception Regression Test
    Given the robot and client are running
    When a TERM signal is sent to both server and client
    Then both applications shut down
    And the log is free of exception messages
    