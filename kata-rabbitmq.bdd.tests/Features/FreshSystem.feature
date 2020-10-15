Feature: Fresh System
    In order to prevent unexpected side effects while testing
    As a developer
    I want to setup the test-infrastructure correctly.

Scenario: Fresh RabbitMQ
    Given a fresh system is installed
    When exchanges are queried
    Then there should exist only the RabbitMQ default exchanges
