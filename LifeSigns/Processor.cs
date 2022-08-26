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

            if (DateTime.Now.Minute < 41)
            {
                readings.HeartRate += 35;
                readings.SpO2 += -5;
                readings.Temperature = readings.Temperature + new Decimal(1.2);
            }
            else if (DateTime.Now.Minute < 59)
            {
                readings.HeartRate += 75;
                readings.SpO2 += -11;
                readings.Temperature = readings.Temperature + new Decimal(3.5);
            }

            readings.HeartRate += random.Next(0, 40) - 20;
            readings.SpO2 += random.Next(0, 40) - 20;
            readings.Temperature += new Decimal(random.NextDouble() - 0.5);

            if (readings.SpO2 > 99) readings.SpO2 = 99;

            GetBloodPressure(random, readings);

            return Round(readings);
        }

        private void GetBloodPressure(Random random, Readings readings)
        {
            if (DateTime.Now.Minute % 20 == 0)
            {
                readings.Systolic = 130;
                readings.DiaStolic = 80;
                readings.DiaStolic += random.Next(0, 40) - 20;
                readings.Systolic += random.Next(0, 40) - 20; 
            }
        }

        private Readings Round(Readings readings)
        {
            if (readings.DiaStolic.HasValue && readings.Systolic.HasValue)
            {
                readings.DiaStolic = Math.Round(readings.DiaStolic.Value, 1);
                readings.Systolic = Math.Round(readings.Systolic.Value, 1);
            }

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

                    Thread.Sleep(900);
                }
            }
        }
    }
}
