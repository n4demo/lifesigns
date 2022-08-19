using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System.Configuration;

namespace LifeSigns
{
    internal class Processor
    {
        private string? eventHubConnectionString;

        private string? eventHubName;

        private EventHubProducerClient? eventHubProducerClient;

        private ValueTask<EventDataBatch>? eventDataBatch;

        private Readings GenerateReadings()
        {
            var readings = new Readings();
            var random = new Random();
            int randomInt = random.Next(0,50) - 25;
            double randomDouble = random.NextDouble() - 0.5;

            int second = DateTime.Now.Second;

            if (second < 16)
            { }
            else if (second < 41)
            {
                readings.DiaStolic += 20;
                readings.Systolic += 25;
                readings.HeartRate += 35;
                readings.SpO2 += -11;
                readings.Temperature += 1.8;
            }
            else if (second < 59)
            {
                readings.DiaStolic += 35;
                readings.Systolic += 57;
                readings.HeartRate += 75;
                readings.SpO2 += -18;
                readings.Temperature += 3.5;
            }

            readings.DiaStolic += randomInt;
            readings.Systolic += randomInt;
            readings.HeartRate += randomInt;
            readings.SpO2 += randomDouble;
            readings.Temperature += randomDouble;

            if (readings.SpO2 > 99) readings.SpO2 = 99;

            return Round(readings);
        }

        private Readings Round(Readings readings)
        {
            readings.DiaStolic = Math.Round(readings.DiaStolic, 1);
            readings.Systolic = Math.Round(readings.Systolic, 1);
            readings.Temperature = Math.Round(readings.Temperature, 2);
            readings.SpO2 = Math.Round(readings.SpO2, 2);
            return readings; 
        }

        public async Task SendData()
        {          
            var enabledString = ConfigurationManager.AppSettings["enabled"];

            bool enabled = false;

            bool.TryParse(enabledString, out enabled);

            if (enabled)
            {
                eventHubConnectionString = ConfigurationManager.AppSettings["eventHubConnectionString"];

                eventHubName = ConfigurationManager.AppSettings["eventHubName"];

                eventHubProducerClient = new EventHubProducerClient(eventHubConnectionString, eventHubName);

                eventDataBatch = eventHubProducerClient.CreateBatchAsync();

                while (true)
                {
                    var readings = GenerateReadings();

                    string json = JsonConvert.SerializeObject(readings);

                    Console.WriteLine(json);

                    var eventData = new EventData(json);

                    if (eventDataBatch.HasValue && !eventDataBatch.Value.Result.TryAdd(eventData))
                    {
                        throw new Exception();
                    }

                    try
                    {
                        await eventHubProducerClient.SendAsync(eventDataBatch.Value.Result);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                    Thread.Sleep(90);
                }
            }
        }
    }
}
