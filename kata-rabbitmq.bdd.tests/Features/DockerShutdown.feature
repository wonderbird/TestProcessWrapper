@DockerShutdown
Feature: Docker Shutdown
  In order to test the server in parallel with clients
  As a developer
  I want to run the server and client in as a docker container

  Scenario: Docker shutdown
    Given the server and client are running
    When a TERM signal is sent to both server and client
    Then both applications shut down
