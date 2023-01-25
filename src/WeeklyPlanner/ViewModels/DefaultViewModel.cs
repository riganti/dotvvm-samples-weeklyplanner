using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using System;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;

namespace WeeklyPlanner.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
        public override async Task Init()
        {
            await Context.Authorize();

            await base.Init();
        }
        public DateTime CurrentDate { get; set; } = DateTime.Today;

        public string UserName => Context.HttpContext.User.Identity.Name;

        public TaskDetailDialogViewModel AddDialog { get; set; } = new TaskDetailDialogViewModel();

        public TaskDetailDialogViewModel EditDialog { get; set; } = new TaskDetailDialogViewModel();



        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await Context.GetAuthentication().SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            Context.RedirectToRoute(Context.Route.RouteName);
        }

    }
}
