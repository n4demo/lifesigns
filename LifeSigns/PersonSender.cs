using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using System.Configuration;

namespace LifeSigns
{
    internal class PersonSender
    {
        public void SendData()
        {          
            var enabledString = ConfigurationManager.AppSettings["enabled"];

            bool enabled = false;

            bool.TryParse(enabledString, out enabled);

            if (enabled)
            {
                var personDetails = new PersonDetails
                {
                    Firstname = "Thomas",
                    LastName = "AnderSon"
                };

                personDetails.Addresses.Add(
                    new Address
                    {
                        Line1 = "100 Some Street",
                        Line2 = "Unit 1",
                        City = "Seattle",
                        State = "WA",
                        Zip = "98012"
                    }
                );

                personDetails.ContactDetails.Add
                    (
                        new ContactDetails
                        {
                            Email = "thomas@andersen.com",
                            Phone = "+1 555 555-5555",
                            Extension = "5555"
                            
                        }
                    );
            }
        }
    }
}
