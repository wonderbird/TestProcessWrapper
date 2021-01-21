@LightSensorReadings
Feature: Light Sensor Readings
    In order to steer my robot
    As a robot owner
    I want to see light sensor readings on the client.
    
  Scenario: 1 client receives all light sensor readings
    Given the robot and 1 client are running
    When the robot has sent at least 1 sensor value
    Then each client received all sent sensor values

  @Ignore
  Scenario: 3 clients receive light sensor readings
    Given the robot and 3 clients are running
    When the robot has sent at least 1 sensor value
    Then each client received all sent sensor values
    