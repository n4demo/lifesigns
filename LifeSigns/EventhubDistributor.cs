using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs;

namespace LifeSigns
{
    internal class EventhubDistributor
    {
        private List<EventHubProducerClient> eventHubProducerClients;
    
        public EventhubDistributor()
        {
            eventHubProducerClients = new List<EventHubProducerClient>
            {
                new EventHubProducerClient(
                    "Endpoint=sb://h4u-use-ehns-lifesigns-nigel.servicebus.windows.net/;SharedAccessKeyName=pol-lifesigns;SharedAccessKey=HdSzcmhyJqncHIvmnEYaWNkznXi+gpQUuVno4COaiRc=;EntityPath=eh-lifesigns-nigel", "eh-lifesigns-nigel")
            };
        }

        public async Task Post(LifesignsReadings lifesignsReadings)
        {
            foreach (var eventHubProducerClient in eventHubProducerClients)
            {
                var eventDataBatch = eventHubProducerClient.CreateBatchAsync();

                string json = JsonConvert.SerializeObject(lifesignsReadings);

                var eventData = new EventData(json);

                try
                {
                    if (!eventDataBatch.Result.TryAdd(eventData))
                    {
                        throw new Exception($"Event is too large for the batch and cannot be sent.");
                    }

                    await eventHubProducerClient.SendAsync(eventDataBatch.Result);

                    Console.WriteLine(json);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }
    }
}
