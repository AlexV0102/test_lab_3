
Feature: Shorten URL
@CleanURI
    Scenario: Shorten a given URL
    Given connect to https://cleanuri.com/api
    And create POST request to v1/shorten
    And add JSON body with original URL https://amath.lp.edu.ua/
    When send request
    Then response is Ok
    And response contains field result_url
