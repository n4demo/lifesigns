﻿namespace LifeSigns
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lifesignsPublisher = new LifesignsPublisher();

                await lifesignsPublisher.PublishLifeSigns();

                //await PublishPerson();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


       static async Task PublishPerson()
        {
            var personPublisher = new PersonPublisher();

            await personPublisher.Init();

            await personPublisher.PublishThomasAnderson();

            for (int i = 0; i < 10; i++)
            {
                await personPublisher.PublishThomasAnderson();
            }

            await personPublisher.PublishRandomPerson();
        }
    }
}
