namespace LifeSigns
{
    public class Readings
    {
        public DateTime When { get; set; }

        public string? Who { get; set; }

        public string? Id { get; set; }

        public int HeartRate { get; set; }

        public decimal? Systolic { get; set; }

        public decimal? DiaStolic { get; set; }

        public decimal SpO2 { get; set; }

        public decimal Temperature { get; set; }

        public Readings()
        {
            HeartRate = 120;
            SpO2 = 97;
            Temperature = 37;
            When = DateTime.Now;
            Who = "Jim Jones";
            Id = Guid.NewGuid().ToString();
        }

    }
}
