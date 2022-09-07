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

                await personSender.Init();

                //await personSender.SaveThomasAnderson();

                //for (int i = 0; i < 100; i++)
                //{
                //    await personSender.SaveThomasAnderson();
                //}


                personSender.SaveRandomPerson();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
