namespace LifeSigns
{
    public class PersonDetails
    {
        public string? Id { get; set; }
        public string? Firstname { get; set; }

        public string? LastName { get; set; }

        public List<Address> Addresses { get; set; }

        public List<ContactDetails> ContactDetails { get; set; }

        public PersonDetails()
        {
            Addresses = new List<Address>();

            ContactDetails = new List<ContactDetails>();
        }
    }

    public class Address
    {
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }

    public class ContactDetails
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Extension { get; set; }
    }
}
