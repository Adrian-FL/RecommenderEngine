using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Recommender.Models;

namespace RecommenderUtils.DAL
{
    public class EventsRepository
    {
        public static void SaveEvent(Event @event)
        {
            using (var session = RavenDb.Store.OpenSession())
            {
                session.Store(@event);
                session.SaveChanges();
            }
        }

        public static List<Event> GetAll()
        {
            using (var session = RavenDb.Store.OpenSession())
            {
                return session.Query<Event>().ToList<Event>();
            }
        }

        public static IEnumerable<EventsByUserByType> GetMapReduce(int minimumEvents = 15)
        {
            using var session = RavenDb.Store.OpenSession();
            return session.Query<EventsByUserByType, Events_ByUserByType>().Where(x => x.Count > minimumEvents).ToList();
        }

        public async static Task<IEnumerable<EventsByUserByType>> GetMapReduceAsync(int minimumEvents = 15)
        {
            using var session = RavenDb.Store.OpenAsyncSession();
            return await session.Query<EventsByUserByType, Events_ByUserByType>().Where(x => x.Count > minimumEvents).ToListAsync();
        }

        public static UserStats GetUserMapReduce(int userId)
        {
            List<EventsByUserByType> queryResult;
            using (var session = RavenDb.Store.OpenSession())
            {
                queryResult = session.Query<EventsByUserByType, Events_ByUserByType>().Where(x => x.UserId == userId).ToList();
            }

            return new UserStats()
            {
                UserId = queryResult.First().UserId,
                ItemsAddedToCart = queryResult.FirstOrDefault(y => y.EventType == "addedtocart")?.Count ?? 0,
                ItemsPurchased = queryResult.FirstOrDefault(y => y.EventType == "transaction")?.Count ?? 0,
                ItemsViewed = queryResult.FirstOrDefault(y => y.EventType == "view")?.Count ?? 0,
            };
        }
    }

    public class Events_ByUserByType : AbstractIndexCreationTask<Event, EventsByUserByType>
    {
        public Events_ByUserByType()
        {
            Map = events => from @event in events
                            select new EventsByUserByType
                            {
                                UserId = @event.UserId,
                                EventType = @event.EventType,
                                Count = 1,
                            };

            Reduce = results => from result in results
                                group result by new { result.UserId, result.EventType } into g
                                select new EventsByUserByType
                                {
                                    UserId = g.Key.UserId,
                                    EventType = g.Key.EventType,
                                    Count = g.Sum(x => x.Count)
                                };
        }
    }
}