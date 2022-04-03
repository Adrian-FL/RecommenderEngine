using Recommender.Models;
using RecommenderUtils.Models;

namespace RecommenderUtils.Helpers
{
    public static class EventsHelper
    {

        public static EventType ParseEventType(string eventType)
        {
            switch (eventType)
            {
                case "view":
                    return EventType.View;
                case "addtocart":
                    return EventType.AddToCart;
                case "transaction":
                    return EventType.Transaction;
            }
            throw new ArgumentException(nameof(eventType));
        }
    }
}
