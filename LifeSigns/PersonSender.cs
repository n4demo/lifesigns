using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Configuration;

namespace LifeSigns
{
    internal class PersonSender
    {
        private ICosmosDbService _cosmosDbService = null;


        public async Task InitSendData()
        {          
            string databaseName = ConfigurationManager.AppSettings["DatabaseName"];

            string containerName = ConfigurationManager.AppSettings["ContainerName"];

            string account = ConfigurationManager.AppSettings["Account"];

            string key = ConfigurationManager.AppSettings["Key"];

            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);

            _cosmosDbService = new CosmosDbService(client, databaseName, containerName);

            Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
        }

        public async Task SendData()
        {
            await InitSendData();

            var personDetails = GetPersonDetails();
        }

        //private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        //{
        //    string databaseName = configurationSection.GetSection("DatabaseName").Value;
        //    string containerName = configurationSection.GetSection("ContainerName").Value;
        //    string account = configurationSection.GetSection("Account").Value;
        //    string key = configurationSection.GetSection("Key").Value;
        //    Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
        //    CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
        //    Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        //    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

        //    return cosmosDbService;
        //}

        private static Random random = new Random();

        public static string RandomString()
        {
            return RandomString(10);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private PersonDetails GetPersonDetails()
        {
            //var random = new Random();
            //var myint = random.Next(11111, 99999);
            
            Thread.Sleep(1100);
            
            PersonDetails personDetails = new PersonDetails
                {
                    Id = "eyAQX05sz*6y8osoh&Ib#&6hD#F",
                    Firstname = $"{RandomString()}",
                    LastName = $"{RandomString()}"
            };

            personDetails.Addresses.Add(
                new Address
                {
                    Line1 = $"{RandomString()} Some Street",
                    Line2 = "Unit 1",
                    City = "Seattle",
                    State = "WA",
                    Zip = "98012"
                }
            );

            personDetails.ContactDetails.Add
                (
                    new ContactDetails
                    {
                        Email = "thomas@andersen.com",
                        Phone = "+1 555 555-5555",
                        Extension = "5555"

                    }
                );

            personDetails.Logins.Add
                (
                    new Login
                    {
                        When = DateTime.Now.ToLongDateString()
                    }
                );

            return personDetails;
        }
    }
}
