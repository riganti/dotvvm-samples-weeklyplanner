using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeklyPlanner.Data
{
    public class User
    {

        public int Id { get; set; }

        [StringLength(100)]
        public string ObjectId { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }

        public ICollection<ScheduledTask> ScheduledTasks { get; } = new List<ScheduledTask>();


    }
}