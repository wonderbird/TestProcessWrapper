@SaveReportBeneathCurrentProject
Feature: Save Report Beneath Current Project
  In order to have a clean project structure
  As a developer using the NuGet package
  I want generated coverage reports to be stored in the TestResults folder of the current project
  
  Scenario: Generate coverage report in TestResults folder of current project
    Given the number of coverage reports in the TestResults folder is known
    And 1 application is running with coverlet 'enabled'
    When a TERM signal is sent to all applications
    Then an additional coverage report exists in the TestResults folder
    