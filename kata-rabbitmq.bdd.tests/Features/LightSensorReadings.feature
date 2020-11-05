@LightSensorReadings
Feature: Light Sensor Readings
    In order to steer my robot
    As a robot owner
    I want to see light sensor readings.

Scenario: Light Sensor Readings
    Given the robot app is started
    When the sensor queue is checked
    Then the sensor queue exists
