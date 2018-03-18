using System;
using System.Collections.Generic;
using System.Linq;
using DotVVM.Framework.ViewModel;
using WeeklyPlanner.DTO;

namespace WeeklyPlanner.ViewModels
{
    public class AddDialogViewModel : DotvvmViewModelBase
    {

        public bool IsDisplayed { get; set; }

        public ScheduledTaskDTO Task { get; set; } = new ScheduledTaskDTO()
        {
            Tags = new List<string>()
        };

    }
}