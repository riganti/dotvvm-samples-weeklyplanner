using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeeklyPlanner.Data
{
    public class ScheduledTask
    {

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public ICollection<ScheduledTaskTransfer> Transfers { get; } = new List<ScheduledTaskTransfer>();

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
