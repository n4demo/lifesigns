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
                var lifesignsReadings = new LifesignsReadings { Systolic = 120, DiaStolic = 80 };

                lifesignsReadings.Who.FullName = person.Fullname;

                person.Readings.Add(lifesignsReadings);

                await cosmosDbRepository.UpsertItemAsync(person);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
