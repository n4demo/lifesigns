using Newtonsoft.Json;

namespace LifeSigns
{
    public class LifesignsReadings
    {
        [JsonProperty(PropertyName = "when")]
        public DateTime When { get; set; }

        [JsonProperty(PropertyName = "who")]
        public Who Who { get; set; }

        [JsonProperty(PropertyName = "heartRate")]
        public int HeartRate { get; set; }

        [JsonProperty(PropertyName = "systolic")]
        public decimal? Systolic { get; set; }

        [JsonProperty(PropertyName = "diastolic")]
        public decimal? DiaStolic { get; set; }

        [JsonProperty(PropertyName = "spo2")]
        public decimal SpO2 { get; set; }

        [JsonProperty(PropertyName = "temperature")]
        public decimal Temperature { get; set; }

        private Random random;

        public LifesignsReadings()
        {
            random = new Random();
            HeartRate = 120 + random.Next(0, 60) - 30;
            SpO2 = 90 + random.Next(0, 10) ;
            Temperature = 36 + random.Next(0, 2) ;
            When = DateTime.Now;
            Who = new Who { FullName = "Jim Jones", UserId="12345454456"};
            Systolic = 130 + random.Next(0, 30) - 15; ;
            DiaStolic = 85 + random.Next(0, 20) - 10; ;
        }
    }

    public class Who
    {
        [JsonProperty(PropertyName = "fullName")]
        public string? FullName { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string? UserId { get; set; }
    }
}
