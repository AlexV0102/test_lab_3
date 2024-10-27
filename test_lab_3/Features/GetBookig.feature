Feature: GetBookingIds

Scenario: Get all booking IDs
    Given connect to https://restful-booker.herokuapp.com
    And create GET request to booking
    When send request
    Then response is Ok
    And response contains booking IDs