using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;

namespace SpecFlowRestApi.Steps
{
    [Binding]
    public class ApiSteps
    {
        protected RestClient client;
        protected RestRequest request;
        protected RestResponse response;
        protected int bookingId;

        [Given(@"connect to (.*)")]
        public void GivenConnectTo(string url)
        {
            client = new RestClient(url);
        }

        [Given(@"create (.*) request to (.*)")]
        public void GivenCreateRequest(string method, string url)
        {
            Method restMethod = method.ToUpper() switch
            {
                "GET" => Method.Get,
                "POST" => Method.Post,
                "DELETE" => Method.Delete,
                "PUT" => Method.Put,
                _ => throw new ArgumentException($"Unsupported HTTP method: {method}")
            };

            request = new RestRequest(url, restMethod);
        }
        [Given(@"create booking")]
        public void GivenCreateBooking()
        {
            GivenConnectTo("https://restful-booker.herokuapp.com");
            GivenCreateRequest("POST", "booking");

            request.AddHeader("Accept", "application/json");
            var booking = new
            {
                firstname = "Dmytro",
                lastname = "Dmytrovich",
                totalprice = 213,
                depositpaid = true,
                bookingdates = new
                {
                    checkin = "2020-01-01",
                    checkout = "2021-01-01"
                },
                additionalneeds = "Breakfast"
            };
            request.AddJsonBody(booking);

            response = client.Execute(request);
            var jsonResponse = JObject.Parse(response.Content);
            bookingId = jsonResponse["bookingid"].Value<int>();
        }

        [Given(@"set parameter id")]
        public void GivenSetParameterID()
        {
            request.AddParameter("id", bookingId, ParameterType.UrlSegment);
        }

        [Given(@"add authorization token")]
        public void GivenAddAuthorizationToken()
        {
            var authRequest = new RestRequest("auth", Method.Post);
            authRequest.AddJsonBody(new
            {
                username = "admin",
                password = "password123"
            });

            var authResponse = client.Execute(authRequest);
            var authResponseObject = JObject.Parse(authResponse.Content);
            string token = authResponseObject["token"].ToString();

            request.AddHeader("Cookie", $"token={token}");
        }


        [Given(@"add header (.*) with value (.*)")]
        public void GivenAddHeader(string header, string value)
        {
            request.AddHeader(header, value);
        }

        [Given(@"add parameter (.*) with value (.*)")]
        public void GivenAddParameter(string parameter, string value)
        {
            request.AddParameter(parameter, value);
        }

        [When(@"send request")]
        public void WhenSendRequest()
        {
            response = client.Execute(request);
            Assert.IsNotNull(response, "Response is null");
        }

        [Then(@"response is (.*)")]
        public void ThenResponseIs(string expectedResponseType)
        {
            HttpStatusCode expectedStatusCode = expectedResponseType.ToUpper() switch
            {
                "OK" => HttpStatusCode.OK,
                "CREATED" => HttpStatusCode.Created,
                _ => throw new ArgumentException($"Unsupported response type: {expectedResponseType}")
            };

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }


        [Given(@"add json with booking")]
        public void GivenAddJsonWithBooking()
        {
            var booking = new
            {
                firstname = "Dmytro",
                lastname = "Dmytrovich",
                totalprice = 111,
                depositpaid = true,
                bookingdates = new
                {
                    checkin = "2020-01-01",
                    checkout = "2021-01-01"
                },
                additionalneeds = "Breakfast"
            };
            request.AddJsonBody(booking);
        }
        [Then(@"response contains json with booking")]
        public void ThenResponseContainsJsonWithBooking()
        {
            var jsonResponse = JObject.Parse(response.Content);
            Assert.AreEqual("Dmytro", (string)jsonResponse["firstname"]);
            Assert.AreEqual("Dmytrovich", (string)jsonResponse["lastname"]);
            Assert.AreEqual(111, (int)jsonResponse["totalprice"]);
            Assert.IsTrue((bool)jsonResponse["depositpaid"]);
            Assert.AreEqual("2020-01-01", (string)jsonResponse["bookingdates"]["checkin"]);
            Assert.AreEqual("2021-01-01", (string)jsonResponse["bookingdates"]["checkout"]);
            Assert.AreEqual("Breakfast", (string)jsonResponse["additionalneeds"]);
        }


        [Then(@"response contains json with booking and booking ID")]
        public void ThenResponseContainsJsonWithBookingAndBookingID()
        {
            var jsonResponse = JObject.Parse(response.Content);
            Assert.IsTrue(jsonResponse.ContainsKey("bookingid"), "Response does not contain booking ID");

            var booking = jsonResponse["booking"];
            Assert.AreEqual("Dmytro", (string)booking["firstname"]);
            Assert.AreEqual("Dmytrovich", (string)booking["lastname"]);
            Assert.AreEqual(111, (int)booking["totalprice"]);
            Assert.IsTrue((bool)booking["depositpaid"]);
            Assert.AreEqual("2020-01-01", (string)booking["bookingdates"]["checkin"]);
            Assert.AreEqual("2021-01-01", (string)booking["bookingdates"]["checkout"]);
            Assert.AreEqual("Breakfast", (string)booking["additionalneeds"]);
        }

        [Then(@"response contains booking IDs")]
        public void ThenResponseContainsBookingIDs()
        {
            var jsonResponse = JArray.Parse(response.Content);
            foreach (var item in jsonResponse)
            {
                Assert.IsTrue(item["bookingid"].Type == JTokenType.Integer);
                Assert.GreaterOrEqual((int)item["bookingid"], 0);
            }
        }

    }
}
