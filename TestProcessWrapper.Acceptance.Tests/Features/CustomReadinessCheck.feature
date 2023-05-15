@CustomReadinessCheck
Feature: Custom Readiness Check
  In order to consider my own preconditions during process startup
  As a test developer using TestProcessWrapper
  I want the startup to be delayed until my custom readiness check is ready.
  
  Scenario: Custom readiness check
    Given A long lived application was wrapped into TestProcessWrapper
    And a custom readiness check was configured 
    When the application is ready
    Then the custom readiness check was executed successfully
  