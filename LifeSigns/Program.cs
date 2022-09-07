namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lifesignsPublisher = new LifesignsPublisher();


                await lifesignsPublisher.PublishThomasAndersonLifeSigns();

                var personPublisher = new PersonPublisher();

                personPublisher.Init();

                await personPublisher.PublishThomasAnderson();

                for (int i = 0; i < 100; i++)
                {
                    await personPublisher.PublishThomasAnderson();
                }


                await personPublisher.PublishRandomPerson();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
