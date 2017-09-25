using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeklyPlanner.DTO;
using WeeklyPlanner.Services;

namespace WeeklyPlanner.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    public class ScheduledTasksController : ControllerBase
    {
        private readonly UserService userService;
        private readonly ScheduledTaskService scheduledTaskService;

        public ScheduledTasksController(UserService userService, ScheduledTaskService scheduledTaskService)
        {
            this.userService = userService;
            this.scheduledTaskService = scheduledTaskService;
        }

        [HttpGet]
        public WeekViewDTO GetWeek(DateTime date)
        {
            var user = userService.GetOrCreateUser((ClaimsIdentity) User.Identity);
            return scheduledTaskService.GetWeek(user, date);
        }

        [HttpPost]
        public void CreateTask([FromBody]ScheduledTaskDTO task)
        {
            var user = userService.GetOrCreateUser((ClaimsIdentity)User.Identity);
            scheduledTaskService.CreateTask(user, task);
        }

        [HttpPut]
        public void UpdateTask([FromBody]ScheduledTaskDTO task)
        {
            var user = userService.GetOrCreateUser((ClaimsIdentity)User.Identity);
            scheduledTaskService.UpdateTask(user, task);
        }

        [HttpPost]
        [Route("move/{id}")]
        public void MoveTask(int id, DateTime date)
        {
            var user = userService.GetOrCreateUser((ClaimsIdentity)User.Identity);
            scheduledTaskService.MoveTask(user, id, date);
        }

        [HttpPost]
        [Route("complete/{id}")]
        public void CompleteTask(int id)
        {
            var user = userService.GetOrCreateUser((ClaimsIdentity)User.Identity);
            scheduledTaskService.CompleteTask(user, id);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public void DeleteTask(int id)
        {
            var user = userService.GetOrCreateUser((ClaimsIdentity)User.Identity);
            scheduledTaskService.DeleteTask(user, id);
        }

    }
}