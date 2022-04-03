using Recommender.Models;
using RecommenderUtils.Helpers;

namespace RecommenderUtils.Engine
{
    public class KNNService
    {
        public static IEnumerable<int> GetNearestUsers(UserStats targetUser, Dictionary<int, UserStats> usersData, HashSet<int> users)
        {
            List<UserDistance> distancesList = new();

            foreach (var userId in users)
            {
                if (!usersData.ContainsKey(userId))
                    continue;

                distancesList.Add(new UserDistance()
                {
                    Distance = RecommenderEngineHelper.EuclidianDistance(targetUser, usersData[userId]),
                    UserId = userId,
                });
            }

            distancesList.Sort();

            return distancesList.Take(10).Select(x => x.UserId);
        }
    }

    internal class UserDistance : IComparable<UserDistance>
    {
        public int UserId { get; set; }
        public double Distance { get; set; }

        public int CompareTo(UserDistance? other)
        {
            if (other == null)
                return 0;

            return Distance.CompareTo(other.Distance);
        }
    }
}
