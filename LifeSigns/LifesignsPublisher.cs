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
            var lifesignsReadings = new LifesignsReadings();

            while (true)
            {
                await eventhubDistributor.Post(lifesignsReadings);

                Thread.Sleep(10000);
            }
        }
    }
}
