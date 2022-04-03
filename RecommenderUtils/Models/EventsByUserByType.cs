namespace Recommender.Models
{
    public class EventsByUserByType
    {
        public int UserId { get; set; }
        public int Count { get; set; }
        public string EventType { get; set; }
        public List<int> ItemIds { get; set; }
    }

    public enum EventType : sbyte
    {
        View = 0,
        AddToCart = 1,
        Transaction = 2
    }
}
