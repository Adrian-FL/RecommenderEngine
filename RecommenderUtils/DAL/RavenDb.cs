using Raven.Client.Documents;

namespace RecommenderUtils.DAL
{
    public static class RavenDb
    {
        public static DocumentStore Store { get; set; }
        public static void Init()
        {
            Store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Recommender"
            };

            Store.Initialize();

            DeployIndexes();
        }

        public static void DeployIndexes()
        {
            new Events_ByUserByType().Execute(Store);
            new Items_ByPopularity().Execute(Store);
            new Items_ByUserId().Execute(Store);
        }
    }
}
