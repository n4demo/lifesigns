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

        private LifesignsReadings GenerateReadings(LifesignsReadings lifesignsreadings)
        {
            var random = new Random();

            if (lifesignsreadings.HeartRate < 60 || lifesignsreadings.HeartRate > 170)
            {
                lifesignsreadings.HeartRate = 100;
            }

            lifesignsreadings.HeartRate += random.Next(0, 20) - 10;

            if (lifesignsreadings.SpO2 > 99 || lifesignsreadings.SpO2 < 60)
            {
                lifesignsreadings.SpO2 = 99;
            }

            lifesignsreadings.SpO2 += random.Next(0, 20) - 10;

            if (lifesignsreadings.Temperature > 41 || lifesignsreadings.Temperature < 34)
            {
                lifesignsreadings.Temperature = 37;
            }

            lifesignsreadings.Temperature = lifesignsreadings.Temperature + new Decimal(random.NextDouble() - 0.5);

            
            GetBloodPressure(random, lifesignsreadings);

            return Round(lifesignsreadings);
        }

        private void GetBloodPressure(Random random, LifesignsReadings lifesignsreadings)
        {
            lifesignsreadings.Systolic = 130;
            lifesignsreadings.DiaStolic = 85;

            lifesignsreadings.Systolic += random.Next(0, 30) - 15; ;
            lifesignsreadings.DiaStolic += random.Next(0, 20) - 10; ;
        }

        private LifesignsReadings Round(LifesignsReadings lifesignsreadings)
        {
            if (lifesignsreadings.DiaStolic.HasValue && lifesignsreadings.Systolic.HasValue)
            {
                lifesignsreadings.DiaStolic = Math.Round(lifesignsreadings.DiaStolic.Value, 1);
                lifesignsreadings.Systolic = Math.Round(lifesignsreadings.Systolic.Value, 1);
            }

            lifesignsreadings.Temperature = Math.Round(lifesignsreadings.Temperature, 2);
            lifesignsreadings.SpO2 = Math.Round(lifesignsreadings.SpO2, 2);
            return lifesignsreadings; 
        }

        public async Task PublishThomasAndersonLifeSigns()
        {          
            if (enabled)
            {
                eventDataBatch = eventHubProducerClient.CreateBatchAsync();

                var readings = new LifesignsReadings();

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
