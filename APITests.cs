using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;
using System.Net;

namespace TestApiTask
{
    public class APITests
    {

        const string baseUrl = "https://contactbook.stoyaniv.repl.co";

        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllAPI()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api");
            client.Timeout = 3000;

            //Acte
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void assertFirstContactIsSteveJobs()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/contacts");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var allContacts = new JsonDeserializer().Deserialize<List<listWithResponse>>(response);

            //Assert
            Assert.AreEqual("Steve", allContacts[0].firstName);
            Assert.AreEqual("Jobs", allContacts[0].lastName);
        }
        [Test]
        public void GetContactsByKeyword()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/contacts/search/albert");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var allContacts = new JsonDeserializer().Deserialize<List<listWithResponse>>(response);

            //Assert
            Assert.AreEqual("Albert", allContacts[0].firstName);
            Assert.AreEqual("Einstein", allContacts[0].lastName);
        }
        [Test]
        public void GetInvalidContactsByKeyword()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/contacts/search/#@!sd");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var allContacts = new JsonDeserializer().Deserialize<List<listWithResponse>>(response);

            //Assert
            Assert.AreEqual(null, allContacts[0].firstName);
            Assert.AreEqual(null, allContacts[0].lastName);
        }
        [Test]
        public void CreateContact()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/contacts");
            client.Timeout = 3000;

            //Act
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                firstName = "Stoyan",
                lastName = "Ivanov",
                email = "stoyan@abv.bg",
                phone = "+359 883 577 123",
                comments = "Hello, good luck"
            });
            var response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        }
        [Test]
        public void CreateAndAssertIsVisibleContact()
        {
            //Arange
            var client = new RestClient(baseUrl + "/api/contacts");
            client.Timeout = 3000;


            //Act
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                firstName = "Stoyan1914",
                lastName = "Ivanov",
                email = "stoyan@abv.bg",
                phone = "+359 883 577 123",
                comments = "Please assert me"
            });
            var response = client.Execute(request);

            //Get result
            var getRequest = new RestRequest(Method.GET);
            var getResponse = client.Execute(getRequest);


            //Assert
            var allContacts = new JsonDeserializer().Deserialize<List<listWithResponse>>(getResponse);
            Assert.AreEqual("Stoyan1914", allContacts[allContacts.Count - 1].firstName);

        }
        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}