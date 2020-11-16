Feature: Docker Shutdown
  In order to test the server in paralllel with clients
  As a developer
  I want to run the server in as a docker container.

  Scenario: Docker shutdown
    Given the server is running
    When a TERM signal is sent
    Then the application shuts down.
