Feature: Create Booking

Scenario: Create Booking
    Given connect to https://restful-booker.herokuapp.com
    And create POST request to booking
    And add header Accept with value application/json
    And add json with booking
    When send request
    Then response is Ok
    And response contains json with booking and booking ID