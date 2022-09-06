namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //await new LifesignsSender().SendThomasAndersonLifeSigns();

                var personSender = new PersonSender();

                await personSender.InitCosmosDB();

                await personSender.SaveThomasAnderson();

                await personSender.SaveThomasAnderson();

                for (int i = 0; i < 100; i++)
                {
                    await personSender.SaveThomasAnderson();
                }

                //await new PersonSender().SaveRandomPerson();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
