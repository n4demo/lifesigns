﻿namespace LifeSigns
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

                await personSender.SaveThomasAnderson();

                //await new PersonSender().SaveRandomPerson();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
