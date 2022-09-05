using Newtonsoft.Json;

namespace LifeSigns
{
    public class PersonDetails
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string? Firstname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string? LastName { get; set; }

        [JsonProperty(PropertyName = "addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty(PropertyName = "contactDetails")]
        public List<ContactDetails> ContactDetails { get; set; }

        [JsonProperty(PropertyName = "logins")]
        public List<Login> Logins { get; set; }

        public PersonDetails()
        {
            Addresses = new List<Address>();

            ContactDetails = new List<ContactDetails>();

            Logins = new List<Login>();
        }
    }

    public class Login
    {
        [JsonProperty(PropertyName = "when")]
        public string? When { get; set; }
    }

    public class Address
    {
        [JsonProperty(PropertyName = "line1")]
        public string? Line1 { get; set; }

        [JsonProperty(PropertyName = "line2")]
        public string? Line2 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string? City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string? State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string? Zip { get; set; }
    }

    public class ContactDetails
    {
        [JsonProperty(PropertyName = "email")]
        public string? Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string? Phone { get; set; }

        [JsonProperty(PropertyName = "extension")]
        public string? Extension { get; set; }
    }
}
