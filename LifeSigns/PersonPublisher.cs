using LifeSigns.Model;
using LifeSigns.Repos;

namespace LifeSigns
{
    internal class PersonPublisher
    {
        private CosmosDbRepository? cosmosDbRepository;

        private PersonGenerator? personGenerator;

        public PersonPublisher()
        {
            personGenerator = new PersonGenerator();

            cosmosDbRepository = new CosmosDbRepository();
        }

        public async Task Init()
        {
           await cosmosDbRepository.Init();
        }

        public async Task PublishThomasAnderson
            ()
        {
            await SavePerson(personGenerator.GetThomas());
        }

        public async Task PublishRandomPerson()
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
                    currentPerson.Readings.Add(new LifesignsReadings { Id = currentPerson.Id, Systolic = 120, DiaStolic = 80 });

                    await cosmosDbRepository.UpsertItemAsync(currentPerson);
                }
                else
                {
                    person.Readings.Add(new LifesignsReadings { Id = person.Id, , Systolic = 120, DiaStolic = 80 });

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
