using System;

namespace WeeklyPlanner.Data
{
    public class ScheduledTaskTransfer
    {

        public int Id { get; set; }

        public int ScheduledTaskId { get; set; }

        public virtual ScheduledTask ScheduledTask { get; set; }

        public DateTime TransferDate { get; set; }

        public DateTime OldDueDate { get; set; }

        public DateTime NewDueDate { get; set; }
        
    }
}