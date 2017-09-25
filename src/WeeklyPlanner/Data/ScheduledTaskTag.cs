namespace WeeklyPlanner.Data
{
    public class ScheduledTaskTag
    {

        public int Id { get; set; }

        public int ScheduledTaskId { get; set; }

        public virtual ScheduledTask ScheduledTask { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }

    }
}