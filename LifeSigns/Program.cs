namespace LifeSigns // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var processor = new Processor();

            await processor.Post();
        }
    }
}
