using Recommender.Models;

namespace RecommenderUtils.DAL
{
    public class UserRepository
    {
        public static List<Event> GetAll(int userId, string eventType)
        {
            using (var session = RavenDb.Store.OpenSession())
            {
                return session.Query<Event>().Where(x=>x.UserId == userId && x.EventType == eventType).ToList<Event>();
            }
        }
    }
}
