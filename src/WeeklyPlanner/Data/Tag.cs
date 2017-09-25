using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeklyPlanner.Data
{
    public class Tag
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<ScheduledTaskTag> ScheduledTasks { get; } = new List<ScheduledTaskTag>();

    }
}