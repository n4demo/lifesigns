using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace LifeSigns
{
    internal class PersonSender
    {
        CosmosClient? _client;

        private Container? _container;

        public async Task InitCosmosDB()
        {          
            string? databaseName = ConfigurationManager.AppSettings["DatabaseName"];

            string? containerName = ConfigurationManager.AppSettings["ContainerName"];

            string? accountEndpoint = ConfigurationManager.AppSettings["AccountEndpoint"];

            string? key = ConfigurationManager.AppSettings["Key"];

             _client = new CosmosClient(accountEndpoint, key);

            DatabaseResponse database = await _client.CreateDatabaseIfNotExistsAsync(databaseName);

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

        private async Task SavePerson(Person personDetails)
        {
            var ticks = DateTime.Now.Ticks.ToString();
            
            Console.WriteLine($"saving {personDetails.Firstname} at {ticks}");

            try
            {
                var response = await _container.ReadItemAsync<Person>(personDetails.Id, new PartitionKey(personDetails.Id));

                if (response != null)
                {
                    var thomas = (Person)response.Resource;

                    thomas.Logins.Add(new Login { When = ticks });

                    await _container.UpsertItemAsync<Person>(personDetails, new PartitionKey(personDetails.Id));
                }
            }
            catch (Exception)
            {
                await _container.CreateItemAsync<Person>(personDetails, new PartitionKey(personDetails.Id));
            }
        }

        private Person GetThomasDetails()
        {
            Person personDetails = new Person
            {
                Id = "1234567890",
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
                    new ContactDetail
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
                        When = DateTime.Now.Ticks.ToString()
                    }
                );

            return personDetails;
        }

        private Person GetRandomPersonDetails()
        {                      
            Person personDetails = new Person
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
                    new ContactDetail
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
