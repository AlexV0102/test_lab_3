using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;

namespace SpecFlowRestApi.Steps
{
    [Binding, Scope(Tag = "CleanURI")]
    public class CleanUriSteps : ApiSteps
    {
        [Given(@"add JSON body with original URL (.*)")]
        public void GivenAddJsonBodyWithOriginalUrl(string originalUrl)
        {
            request.AddJsonBody(new { url = originalUrl });
        }

        [Then(@"response contains field (.*)")]
        public void ThenResponseContainsField(string field)
        {
            var jsonResponse = JObject.Parse(response.Content);
            Assert.That(jsonResponse[field], Is.Not.Null, $"Response does not contain field '{field}'");
            Assert.IsNotEmpty(jsonResponse[field]?.ToString(), $"Field '{field}' is empty in the response");
        }
    }
}
