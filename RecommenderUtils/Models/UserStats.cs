namespace Recommender.Models
{
    public class UserStats
    {
        public int UserId { get; set; }
        public int Cluster { get; set; }
        public int ItemsViewed { get; set; }
        public int ItemsAddedToCart { get; set; }
        public int ItemsPurchased { get; set; }
    }
}
