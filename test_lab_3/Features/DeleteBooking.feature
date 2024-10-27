Feature: Delete Booking

A short summary of the feature

@tag1
Scenario: Delete Booking
	Given connect to https://restful-booker.herokuapp.com
	And create DELETE request to booking/{id}
	And create booking
	And set parameter id
	And add authorization token
	When send request
	Then response is Ok