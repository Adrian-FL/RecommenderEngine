namespace Recommender.Models
{
    public class UserEvent
    {
        public int VisitorId { get; set; }
        public int ItemId { get; set; }
        public DateTime DateTime { get; set; }
        public bool Purchase { get; set; }
        public int NumCategoryId { get; set; }
        public int NumParentId { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }
        public int Week { get; set; }
        public int PreviousView { get; set; }
        public int PreviousTransaction { get; set; }

    }
}
