namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await PublishPerson();

                //await PublishLPersonSifeSigns();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        static async Task PublishLPersonSifeSigns()
        {
            var lifesignsPublisher = new LifesignsPublisher();


            await lifesignsPublisher.PublishThomasAndersonLifeSigns();


            await PublishPerson();
        }

       static async Task PublishPerson()
        {
            var personPublisher = new PersonPublisher();

            await personPublisher.Init();

            await personPublisher.PublishThomasAnderson();

            for (int i = 0; i < 100; i++)
            {
                await personPublisher.PublishThomasAnderson();
            }

            await personPublisher.PublishRandomPerson();
        }
    }
}
