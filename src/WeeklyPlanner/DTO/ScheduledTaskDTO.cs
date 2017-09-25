using System;
using System.Collections.Generic;

namespace WeeklyPlanner.DTO
{
    public class ScheduledTaskDTO
    {

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public List<string> Tags { get; set; }

    }
}