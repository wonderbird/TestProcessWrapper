@CorrectUsage
Feature: Correct Usage
  In order have reliable tests
  As a test developer using ProcessWrapper
  I want to know how ProcessWrapper shall be used

  @Ignore
  Scenario: Correct usage: 1 application with coverlet enabled
    Given 1 application is running with coverlet 'enabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the reported total line coverage is greater 0%

  @Ignore
  Scenario: Correct usage: 2 applications, 1 has coverlet enabled, 1 has coverlet disabled
    Given 1 application is running with coverlet 'enabled'
    And 1 application is running with coverlet 'disabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the reported total line coverage is greater 0%

  @Ignore
  Scenario: Correct usage: 2 applications, both have coverlet disabled
    Given 2 applications are running with coverlet 'disabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the log is free of exception messages

  @Ignore
  Scenario: Wrong usage: 2 applications, both have coverlet enabled
    Given 2 applications are running with coverlet 'enabled'
    When a TERM signal is sent to all applications
    Then all applications shut down
    And the reported total line coverage equals 0%
    