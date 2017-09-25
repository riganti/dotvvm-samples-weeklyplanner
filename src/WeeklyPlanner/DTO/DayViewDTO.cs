using System;
using System.Collections.Generic;

namespace WeeklyPlanner.DTO
{
    public class DayViewDTO
    {

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public List<ScheduledTaskDTO> Tasks { get; set; }

    }
}