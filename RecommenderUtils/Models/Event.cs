using Newtonsoft.Json;

namespace Recommender.Models
{
    public class Event
    {
        public string Timestamp { get; set; }
        [JsonProperty("VisitorId")]
        public int UserId { get; set; }
        [JsonProperty("Event")]
        public string EventType { get; set; }
        public int ItemId { get; set; }

        public int? TransactionId { get; set; }

       
    }
}
