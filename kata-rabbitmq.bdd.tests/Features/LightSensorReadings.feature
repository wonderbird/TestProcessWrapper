Feature: Light Sensor Readings
    In order to steer my robot
    As a robot owner
    I want to see light sensor readings on the client.
    
Scenario: Client receives light sensor readings
    Given the robot app is started
    And the client app is started
    When the robot and client app have been connected for 2.5 seconds
    Then the client app received at least 1 sensor values
    