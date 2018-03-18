using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using WeeklyPlanner.Api;
using WeeklyPlanner.DTO;

namespace WeeklyPlanner.ViewModels
{

    [Authorize]
    public class DefaultViewModel : DotvvmViewModelBase
    {

        public DateTime CurrentDate { get; set; } = DateTime.Today;

        public string CurrentWeek => "Week " + (1 + CurrentDate.DayOfYear / 7);

        public string UserName => Context.HttpContext.User.Identity.Name;

        public AddDialogViewModel AddDialog { get; set; } = new AddDialogViewModel();

        public AddDialogViewModel EditDialog { get; set; } = new AddDialogViewModel();


        public void MoveWeek(int direction)
        {
            CurrentDate = CurrentDate.AddDays(7 * direction);
        }


        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await Context.GetAuthentication().SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            Context.RedirectToRoute(Context.Route.RouteName);
        }

    }
}
