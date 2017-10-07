using System;
using System.Collections.Generic;
using System.Linq;
using DotVVM.Framework.ViewModel;

namespace WeeklyPlanner.ViewModels
{
    public class AddDialogViewModel : DotvvmViewModelBase
    {

        public bool IsDisplayed { get; set; }

        public Api.ScheduledTaskDTO Task { get; set; } = new Api.ScheduledTaskDTO()
        {
            Tags = new List<string>()
        };

    }
}