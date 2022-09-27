using Newtonsoft.Json;

namespace LifeSigns
{
    public class LifesignsReadings
    {
        [JsonProperty(PropertyName = "when")]
        public DateTime When { get; set; }

        [JsonProperty(PropertyName = "who")]
        public string? Who { get; set; }

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

        public LifesignsReadings()
        {
            HeartRate = 110;
            SpO2 = 97;
            Temperature = 37;
            When = DateTime.Now;
        }

    }
}
