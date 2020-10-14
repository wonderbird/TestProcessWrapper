Feature: Fresh System
    In order to provision the product
    As a system administrator
    I want to setup the underlying infrastructure correctly.

Scenario: Fresh RabbitMQ
    Given a fresh system is installed
    When exchanges are queried
    Then there should exist only the RabbitMQ default exchanges
