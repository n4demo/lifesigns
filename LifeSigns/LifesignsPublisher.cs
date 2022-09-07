using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System.Configuration;

namespace LifeSigns
{
    internal class LifesignsPublisher
    {
        private string? eventHubConnectionString;

        private string? eventHubName;

        private PersonGenerator personGenerator;

        private EventHubProducerClient? eventHubProducerClient;

        private ValueTask<EventDataBatch>? eventDataBatch;

        private bool enabled;

        public LifesignsPublisher()
        {
            var enabledString = ConfigurationManager.AppSettings["enabled"];

            bool.TryParse(enabledString, out enabled);

            personGenerator = new PersonGenerator();

            eventHubConnectionString = ConfigurationManager.AppSettings["eventHubConnectionString"];

            eventHubName = ConfigurationManager.AppSettings["eventHubName"];

            eventHubProducerClient = new EventHubProducerClient(eventHubConnectionString, eventHubName);
        }

        private Readings GenerateReadings(Readings readings)
        {
            var random = new Random();

            if (readings.HeartRate < 60 || readings.HeartRate > 170)
            {
                readings.HeartRate = 100;
            }

            readings.HeartRate += random.Next(0, 20) - 10;

            if (readings.SpO2 > 99 || readings.SpO2 < 60)
            {
                readings.SpO2 = 99;
            }

            readings.SpO2 += random.Next(0, 20) - 10;

            if (readings.Temperature > 41 || readings.Temperature < 34)
            {
                readings.Temperature = 37;
            }

            readings.Temperature = readings.Temperature + new Decimal(random.NextDouble() - 0.5);

            
            GetBloodPressure(random, readings);

            return Round(readings);
        }

        private void GetBloodPressure(Random random, Readings readings)
        {
            readings.Systolic = 130;
            readings.DiaStolic = 85;

            readings.Systolic += random.Next(0, 30) - 15; ;
            readings.DiaStolic += random.Next(0, 20) - 10; ;
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

        public async Task SendThomasAndersonLifeSigns()
        {          
            if (enabled)
            {
                eventDataBatch = eventHubProducerClient.CreateBatchAsync();

                var readings = new Readings();

                while (true)
                {
                    readings = GenerateReadings(readings);

                    readings.Id = personGenerator.GetThomas().Id;

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

                    Thread.Sleep(10000);
                }
            }
        }
    }
}
