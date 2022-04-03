using Recommender.Models;

namespace RecommenderUtils.Helpers
{
    public static class RecommenderEngineHelper
    {
        public static double EuclidianDistance(UserStats a, UserStats b)
        {
            int d1, d2, d3;
            d1 = a.ItemsViewed - b.ItemsViewed;
            d2 = a.ItemsAddedToCart - b.ItemsAddedToCart;
            d3 = a.ItemsPurchased - b.ItemsPurchased;
            return 1 + Math.Sqrt(d1 * d1 + d2 * d2 + d3 * d3);
        }
    }
}