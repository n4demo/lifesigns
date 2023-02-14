using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System.Configuration;

namespace LifeSigns
{
    internal class LifesignsPublisher
    {
        private EventhubDistributor eventhubDistributor;

        public LifesignsPublisher()
        {
            eventhubDistributor = new EventhubDistributor();
        }

        public async Task PublishLifeSigns()
        {          
            while (true)
            {
                var lifesignsReadings = new LifesignsReadings();

                await eventhubDistributor.Post(lifesignsReadings);

                Thread.Sleep(1000);
            }
        }
    }
}
