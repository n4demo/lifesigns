using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace LifeSigns
{
    internal class PersonSender
    {
        private CosmosDbRepository? cosmosDbRepository;

        public PersonSender()
        {

        }

        public async Task Init()
        {
            cosmosDbRepository = new CosmosDbRepository();

            await cosmosDbRepository.Init();

            return;
        }

        public async Task SaveThomasAnderson()
        {
            await SavePerson(GetThomasDetails());
        }

        public async Task SaveRandomPerson()
        {
            await SavePerson(GetRandomPersonDetails());
        }

        private async Task SavePerson(Person person)
        {
            var ticks = DateTime.Now.Ticks.ToString();
            
            Console.WriteLine($"saving {person.Firstname} at {ticks}");

            try
            {
                var response = cosmosDbRepository.ReadItemAsync(person.Id);

                if (response != null)
                {
                    var thomas = (Person)response.Result;

                    thomas.Logins.Add(new Login { When = ticks });

                    await cosmosDbRepository.UpsertItemAsync(thomas);
                }
            }
            catch (Exception)
            {
                await cosmosDbRepository.UpsertItemAsync(person);
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
