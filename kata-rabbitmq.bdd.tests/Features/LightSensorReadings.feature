Feature: Light Sensor Readings
    In order to steer my robot
    As a robot owner
    I want to see light sensor readings on the client.
    
  Scenario: 1 client receives light sensor readings
    Given the server and 1 client are running
    When the robot and client apps have been connected for 2.5 seconds
    Then each client app received at least 1 sensor values
    
  #@Ignore
  Scenario: 3 clients receive light sensor readings
    Given the server and 3 clients are running
    When the robot and client apps have been connected for 2.5 seconds
    Then each client app received at least 1 sensor values
    