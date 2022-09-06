namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await new LifesignsSender().SendThomasAndersonLifeSigns();

                await new PersonSender().SaveThomasAnderson();

                await new PersonSender().SaveRandomPerson();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
