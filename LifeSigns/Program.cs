namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var processor = new Processor();

                await processor.SendData();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
