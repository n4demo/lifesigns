using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace LifeSigns
{
    internal class PersonSender
    {
        private CosmosDbRepository? cosmosDbRepository;

        private PersonGenerator? personGenerator;

        public PersonSender()
        {
            personGenerator = new PersonGenerator();

            cosmosDbRepository = new CosmosDbRepository();
        }

        public async Task Init()
        {
           await cosmosDbRepository.Init();
        }

        public async Task SaveThomasAnderson()
        {
            await SavePerson(personGenerator.GetThomas());
        }

        public async Task SaveRandomPerson()
        {
            await SavePerson(personGenerator.GetRandomPerson());
        }

        private async Task SavePerson(Person person)
        {
            var ticks = DateTime.Now.Ticks.ToString();
            
            Console.WriteLine($"saving {person.Firstname} at {ticks}");

            try
            {
                var currentPerson = await cosmosDbRepository.ReadItemAsync(person.Id);

                if (currentPerson != null)
                {
                    currentPerson.Logins.Add(new Login { When = ticks });

                    await cosmosDbRepository.UpsertItemAsync(currentPerson);
                }
                else
                {
                    await cosmosDbRepository.UpsertItemAsync(person);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
