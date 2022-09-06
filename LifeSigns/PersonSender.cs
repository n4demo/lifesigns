using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace LifeSigns
{
    internal class PersonSender
    {
        //private ICosmosDbService _cosmosDbService = null;

        Microsoft.Azure.Cosmos.CosmosClient? _client;

        private Container? _container;

        public async Task InitSendData()
        {          
            string? databaseName = ConfigurationManager.AppSettings["DatabaseName"];

            string? containerName = ConfigurationManager.AppSettings["ContainerName"];

            string? account = ConfigurationManager.AppSettings["Account"];

            string? key = ConfigurationManager.AppSettings["Key"];

             _client = new CosmosClient(account, key);

            //_cosmosDbService = new CosmosDbService(_client, databaseName, containerName);

            Microsoft.Azure.Cosmos.DatabaseResponse database = await _client.CreateDatabaseIfNotExistsAsync(databaseName);

            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            _container = _client.GetContainer(databaseName, containerName);
        }

        public async Task SaveThomasAnderson()
        {
            await SavePerson(GetThomasDetails());
        }

        public async Task SaveRandomPerson()
        {
            await SavePerson(GetRandomPersonDetails());
        }

        private async Task SavePerson(PersonDetails personDetails)
        {
            await _container.CreateItemAsync<PersonDetails>(personDetails, new PartitionKey(personDetails.Id));
        }

        private PersonDetails GetThomasDetails()
        {
            PersonDetails personDetails = new PersonDetails
            {
                Id = "eyAQX05sz*6y8osoh&Ib#&6hD#F",
                Firstname = "Thomas",
                LastName = "AnderSon"
            };

            personDetails.Addresses.Add(
                new Address
                {
                    Line1 = "100 Some Street",
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

        private PersonDetails GetRandomPersonDetails()
        {                      
            PersonDetails personDetails = new PersonDetails
                {
                    Id = RandomString(),
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
                        Email = $"{RandomString()}@acme.com",
                        Phone = "+1 555 555-5555",
                        Extension = "5555"

                    }
                );

            personDetails.Logins.Add
                (
                    new Login { When = DateTime.Now.ToLongDateString() }
                );

            return personDetails;
        }

        private static Random random = new Random();

        private static string RandomString()
        {
            return RandomString(6);
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
