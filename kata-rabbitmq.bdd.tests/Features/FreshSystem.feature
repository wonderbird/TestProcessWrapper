Feature: Fresh System
    In order to prevent unexpected side effects while testing
    As a developer
    I want to setup the test-infrastructure correctly.

Scenario: Fresh RabbitMQ
    Given a fresh system is installed
    Then the RabbitMQ channel is open
    
    Given a fresh system is installed
    When robot exchanges are queried
    Then the robot exchanges do not exist
