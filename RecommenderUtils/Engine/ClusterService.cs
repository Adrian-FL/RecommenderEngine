using Recommender.Models;
using RecommenderUtils.DAL;
using Accord.MachineLearning;

namespace RecommenderUtils.Engine
{
    public static class ClusterService
    {
        private static KMeansClusterCollection? _kMeansClusters;

        public async static Task GenerateClustersAsync(Action<List<UserStats>> successCallback)
        {
            // get data from raven db using a map reduce index
            var mapReduceIndex = await EventsRepository.GetMapReduceAsync(1);
            
            List<UserStats> usersStats = mapReduceIndex.GroupBy(x => x.UserId).Select(x => new UserStats()
            {
                UserId = x.Key,
                Cluster = -1,
                ItemsAddedToCart = x.FirstOrDefault(y => y.EventType == "addedtocart")?.Count ?? 0,
                ItemsPurchased = x.FirstOrDefault(y => y.EventType == "transaction")?.Count ?? 0,
                ItemsViewed = x.FirstOrDefault(y => y.EventType == "view")?.Count ?? 0,
            }).ToList();

            // number of clusters
            // not sure how to determine the best value
            var kMeans = new KMeans(17);

            double[][] kMeansData = new double[usersStats.Count][];
            int index = 0;
         
            foreach(var user in usersStats)
            {
                kMeansData[index] = new double[3];
                kMeansData[index][0] = user.ItemsAddedToCart;
                kMeansData[index][1] = user.ItemsViewed;
                kMeansData[index][2] = user.ItemsPurchased;
                index++;
            }

            // Compute and retrieve the data centroids
            _kMeansClusters = kMeans.Learn(kMeansData);
            
            // Use the centroids to partition all the data
            int[] userCluster = _kMeansClusters.Decide(kMeansData);

            index = 0;
            foreach(var user in usersStats)
            {
                user.Cluster = userCluster[index];
                index++;
            }

            successCallback?.Invoke(usersStats);
        }
        
        public static int PredictUser(UserStats userStats)
        {
            if (_kMeansClusters == null)
                return -1;

            var kMeansData = new double[3];
            kMeansData[0] = userStats.ItemsAddedToCart;
            kMeansData[1] = userStats.ItemsViewed;
            kMeansData[2] = userStats.ItemsPurchased;

            return _kMeansClusters.Decide(kMeansData);
        }
    }
}