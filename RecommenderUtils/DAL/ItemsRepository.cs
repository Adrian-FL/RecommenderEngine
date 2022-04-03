using Raven.Client.Documents.Indexes;
using Recommender.Models;
using RecommenderUtils.Models;
using Raven.Client.Documents.Linq;

namespace RecommenderUtils.DAL
{
    public class ItemsRepository
    {
        public static IEnumerable<ItemsByPopularity> GetPopularItems(int top = 10)
        {
            using var session = RavenDb.Store.OpenSession();
            return session.Query<ItemsByPopularity, Items_ByPopularity>().OrderByDescending(x => x.Score).Take(top).ToList();
        }

        public static IEnumerable<ItemsByPopularity> GetPopularItems(int top = 10, params int[] itemsIds)
        {
            using var session = RavenDb.Store.OpenSession();
            return session.Query<ItemsByPopularity, Items_ByPopularity>().Where(x => x.ItemId.In(itemsIds)).OrderByDescending(x => x.Score).Take(top).ToList();
        }

        public static IEnumerable<ItemsByPopularity> GetUserPopularItems(int top = 10, params int[] userIds)
        {
            using var session = RavenDb.Store.OpenSession();
            var items = GetUsersItems(userIds);
            return GetPopularItems(top, items.ToArray());
        }

        public static IEnumerable<int> GetUsersItems(params int[] userIds)
        {
            using var session = RavenDb.Store.OpenSession();

            //untill ToList() every operation is made on IQueryable which means that RavenDB will take the instructions into account when issueing the query on the db
            // otherwise we would apply the instructions on inmemory objects (after they are transmitted over network and deserialised into C# objects which is slower)
            return session.Query<ItemsByUser, Items_ByUserId>().Where(x => x.UserId.In(userIds)).Select(x => x.ItemId).Distinct().ToList();
        }

    }

    public class Items_ByPopularity : AbstractIndexCreationTask<Event, ItemsByPopularity>
    {
        public Items_ByPopularity()
        {
            Map = events => from @event in events
                            select new ItemsByPopularity
                            {
                                ItemId = @event.ItemId,
                                Score = @event.EventType == "transaction" ? 6 : @event.EventType == "addtocart" ? 3 : 1
                            };

            Reduce = results => from result in results
                                group result by result.ItemId into g
                                select new ItemsByPopularity
                                {
                                    ItemId = g.Key,
                                    Score = g.Sum(x => x.Score)
                                };
        }
    }

    public class Items_ByUserId : AbstractIndexCreationTask<Event, ItemsByUser>
    {
        public Items_ByUserId()
        {
            Map = events => from @event in events
                            select new ItemsByUser
                            {
                                ItemId = @event.ItemId,
                                UserId = @event.UserId
                            };

            Reduce = results => from result in results
                                group result by new { result.ItemId, result.UserId } into g
                                select new ItemsByUser
                                {
                                    ItemId = g.Key.ItemId,
                                    UserId = g.Key.UserId
                                };
        }
    }
}