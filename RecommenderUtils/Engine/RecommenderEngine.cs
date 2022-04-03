
using Recommender.Models;
using RecommenderUtils.DAL;

namespace RecommenderUtils.Engine
{
    public static class RecommenderEngine
    {
        #region private fields
        private static bool _isClusterReady = false;

        /// <summary>
        /// UserId, UserData
        /// </summary>
        private static Dictionary<int, UserStats> _userData = new();

        /// <summary>
        /// Cluster, UserId
        /// </summary>
        private static Dictionary<int, HashSet<int>> _clusterData = new();
        #endregion

        #region private methods
        /// <summary>
        /// Method invoked by ClusuterService after finishing cluster generation
        /// The method sets the internal data of Recommender Engine in order to facillitate fast user check up and user data
        /// </summary>
        /// <param name="users"></param>
        private static void SetData(List<UserStats> users)
        {
            _clusterData = new();
            foreach (var user in users)
            {
                if (!_clusterData.ContainsKey(user.Cluster))
                {
                    _clusterData.Add(user.Cluster, new HashSet<int>());
                }

                if (!_clusterData[user.Cluster].Contains(user.UserId))
                    _clusterData[user.Cluster].Add(user.UserId);
            }

            _userData = users.ToDictionary(x => x.UserId, x => x);
            _isClusterReady = true;
        }
       
        private static IEnumerable<int> GetMostPopularItems() => ItemsRepository.GetPopularItems(10).Select(x => x.ItemId);
        #endregion

        /// <summary>
        /// Method invoked by ClusterBackgroundService at application start up
        /// After finishing, the method calls SetData which set internal data and _isClusterReady to true
        /// </summary>
        /// <returns></returns>
        public async static Task GenerateClustersAsync()
        {
            if (_isClusterReady == true)
                return;

            await ClusterService.GenerateClustersAsync(x => SetData(x));
        }

        /// <summary>
        /// For a given userId returns the list of max. 10 recommended items
        /// </summary>
        public static IEnumerable<int> GetRecommendations(int userId)
        {
            if (_isClusterReady == false)
            {
                // return most popular items untill cluster is ready
                return GetMostPopularItems();
            }

            // if user is not in cluster that means user has no associated data (aka cold start)
            // return most popular items in this case
            if (!_userData.ContainsKey(userId))
                return GetMostPopularItems();

            // KNN on users from the same cluster of userId
            var userData = _userData[userId];
            var recommendedUsers = KNNService.GetNearestUsers(userData, _userData, _clusterData[userData.Cluster]);
            return ItemsRepository.GetUserPopularItems(10, recommendedUsers.ToArray()).Select(x => x.ItemId);
        }

        /// <summary>
        /// Update user data internally when a new event is received
        /// User cluster is recalculated
        /// </summary>
        /// <param name="userId"></param>
        public static void UpdateUser(int userId)
        {
            if (_isClusterReady == false)
            {
                //nothing to update until clusters are ready
                return;
            }

            //get all user details
            var userStats = EventsRepository.GetUserMapReduce(userId);

            //get user cluster
            int cluster = ClusterService.PredictUser(userStats);

            //update internal data
            _userData[userId] = userStats;
            if (!_clusterData[cluster].Contains(userId))
                _clusterData[cluster].Add(userId);
        }
    }
}