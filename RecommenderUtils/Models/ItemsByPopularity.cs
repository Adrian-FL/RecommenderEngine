namespace RecommenderUtils.Models
{
    public class ItemsByPopularity
    {
        public int ItemId { get; set; }
        public int Score { get; set; }
    }

    public class ItemsByEvent
    {
        public int ItemId { get; set; }
        public string EvenType { get; set; }
        public int Count { get; set; }
    }
}
