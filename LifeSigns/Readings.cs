namespace LifeSigns
{
    public class Readings
    {
        public DateTime When { get; set; }

        public string? Who { get; set; }

        public string? Id { get; set; }

        public int HeartRate { get; set; }

        public double Systolic { get; set; }

        public double DiaStolic { get; set; }

        public double SpO2 { get; set; }

        public double Temperature { get; set; }

        public Readings()
        {
            HeartRate = 120;
            Systolic = 130;
            DiaStolic = 80;
            SpO2 = 97.0;
            Temperature = 37.5;
            When = DateTime.Now;
            Who = "Jim Jones";
            Id = Guid.NewGuid().ToString();
        }

    }
}
