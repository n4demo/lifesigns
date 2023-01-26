using Newtonsoft.Json;

namespace LifeSigns.Model
{
    public class Person
    {
        [JsonProperty(PropertyName = "fullname")]
        public string? Fullname 
        { 
            get { return String.Format("{0}-{1}", Firstname, LastName).ToLower(); } 
        }

        [JsonProperty(PropertyName = "userId")]
        public string? UserId
        {
            get { return new Guid().ToString(); }
        }

        [JsonProperty(PropertyName = "firstName")]
        public string? Firstname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string? LastName { get; set; }

        [JsonProperty(PropertyName = "addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty(PropertyName = "contactDetails")]
        public List<ContactDetail> ContactDetails { get; set; }

        [JsonProperty(PropertyName = "readings")]
        public List<LifesignsReadings> Readings { get; set; }

        public Person()
        {
            Addresses = new List<Address>();

            ContactDetails = new List<ContactDetail>();

            Readings = new List<LifesignsReadings>();
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

    public class ContactDetail
    {
        [JsonProperty(PropertyName = "email")]
        public string? Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string? Phone { get; set; }

        [JsonProperty(PropertyName = "extension")]
        public string? Extension { get; set; }
    }
}
