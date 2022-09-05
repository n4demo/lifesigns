namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lifesignsSender = new LifesignsSender();

                await lifesignsSender.SendData();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
