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
                    "Endpoint=sb://h4u-use-ehns-lifesigns.servicebus.windows.net/;SharedAccessKeyName=pol-lifesigns;SharedAccessKey=RCRqt4ZbEOE5ucXUooqTYIVBFT5wlGa7P4AqHOIZJAg=;EntityPath=evh-lifesigns", "evh-lifesigns")
            };
        }

        public async Task Post(LifesignsReadings readings)
        {
            foreach (var eventHubProducerClient in eventHubProducerClients)
            {
                try
                {
                    ValueTask<EventDataBatch>? eventDataBatch = eventHubProducerClient.CreateBatchAsync();

                    string json = JsonConvert.SerializeObject(readings);

                    Console.WriteLine(json);

                    var eventData = new EventData(json);

                    await eventHubProducerClient.SendAsync(eventDataBatch.Value.Result);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }
    }
}
