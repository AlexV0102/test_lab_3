Feature: Update Booking

Scenario: Update Booking
	Given connect to https://restful-booker.herokuapp.com
	And create booking
	And create PUT request to booking/{id}
	And set parameter id
	And add header Accept with value application/json
	And add header Content-Type with value application/json
	And add authorization token
	And add json with booking
	When send request
	Then response is Ok
	And response contains json with booking