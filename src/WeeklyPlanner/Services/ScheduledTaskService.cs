using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Data;
using WeeklyPlanner.DTO;

namespace WeeklyPlanner.Services
{
    public class ScheduledTaskService : ServiceBase
    {
        public ScheduledTaskService(AppDbContext context) : base(context)
        {
        }

        public WeekViewDTO GetWeek(UserDTO user, DateTime date)
        {
            var weekBeginDate = GetWeekBeginDate(date);
            var weekEndDate = weekBeginDate.AddDays(7);

            var tasks = Context.ScheduledTasks
                .Where(t => t.DeletedDate == null)
                .Where(t => t.UserId == user.Id)
                .Where(t => t.DueDate >= weekBeginDate && t.DueDate < weekEndDate)
                .Select(t => new ScheduledTaskDTO()
                {
                    Id = t.Id,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    CompletedDate = t.CompletedDate,
                    Text = t.Text
                })
                .ToList();

            var additionalTasks = Context.ScheduledTasks
                .Where(t => t.DeletedDate == null)
                .Where(t => t.UserId == user.Id)
                .Where(t => t.DueDate >= weekEndDate)
                .OrderBy(t => t.DueDate)
                .Take(5)
                .Select(t => new ScheduledTaskDTO()
                {
                    Id = t.Id,
                    CreatedDate = t.CreatedDate,
                    DueDate = t.DueDate,
                    CompletedDate = t.CompletedDate,
                    Text = t.Text
                })
                .ToList();

            var allTaskIds = tasks.Concat(additionalTasks).Select(t => t.Id).ToList();
            

            return new WeekViewDTO()
            {
                Days = Enumerable.Range(0, 7)
                    .Select(i => new { BeginDate = weekBeginDate.AddDays(i), EndDate = weekBeginDate.AddDays(i + 1) })
                    .Select(i => new DayViewDTO()
                    {
                        Date = i.BeginDate,
                        Name = CultureInfo.CurrentUICulture.DateTimeFormat.GetDayName(i.BeginDate.DayOfWeek),
                        Tasks = tasks
                            .Where(t => t.DueDate >= i.BeginDate && t.DueDate < i.EndDate)
                            .OrderBy(t => t.CompletedDate != null).ThenBy(t => t.Id)
                            .ToList()
                    })
                    .Concat(new[]
                    {
                        new DayViewDTO()
                        {
                            Date = weekEndDate,
                            Name = "Next Week",
                            Tasks = additionalTasks
                        }
                    })
                    .ToArray()
            };
        }

        public void CreateTask(UserDTO user, ScheduledTaskDTO task)
        {
            // create task
            var entity = new ScheduledTask()
            {
                CreatedDate = DateTime.UtcNow,
                UserId = user.Id,
                DueDate = task.DueDate,
                Text = task.Text
            };
            Context.ScheduledTasks.Add(entity);

            Context.SaveChanges();
        }

        public void UpdateTask(UserDTO user, ScheduledTaskDTO task)
        {
            var entity = GetTask(user, task.Id);

            // update task and tags
            entity.Text = task.Text;

            Context.SaveChanges();
        }
        
        public void MoveTask(UserDTO user, int id, DateTime date)
        {
            var task = GetTask(user, id);

            var transfer = new ScheduledTaskTransfer()
            {
                ScheduledTaskId = id,
                OldDueDate = task.DueDate,
                NewDueDate = date,
                TransferDate = DateTime.UtcNow
            };
            Context.ScheduledTaskTransfers.Add(transfer);

            task.DueDate = date;

            Context.SaveChanges();
        }

        public void CompleteTask(UserDTO user, int id)
        {
            var task = GetTask(user, id);
            task.CompletedDate = DateTime.UtcNow;
            Context.SaveChanges();
        }

        public void DeleteTask(UserDTO user, int id)
        {
            var task = GetTask(user, id);
            task.DeletedDate = DateTime.UtcNow;
            Context.SaveChanges();
        }


        private DateTime GetWeekBeginDate(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }

        

        private ScheduledTask GetTask(UserDTO user, int id)
        {
            var task = Context.ScheduledTasks.Find(id);
            if (task?.UserId != user.Id)
            {
                throw new UnauthorizedAccessException();
            }
            return task;
        }
    }
}