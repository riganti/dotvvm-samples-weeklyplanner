using System;
using System.Reflection;
using DotVVM.Framework;
using DotVVM.Framework.Compilation.Javascript;
using DotVVM.Framework.Compilation.Javascript.Ast;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.DependencyInjection;
using WeeklyPlanner.Controls;
using WeeklyPlanner.Helpers;

namespace WeeklyPlanner
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        public void ConfigureServices(IDotvvmServiceCollection services)
        {
            services.AddDefaultTempStorages("Temp");
        }

        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);

            config.RegisterApiClient(typeof(Api.Client), "http://localhost:6806/", "wwwroot/apiClient.js", "_api");

            config.Markup.JavascriptTranslator.MethodCollection.AddMethodTranslator(typeof(DateTime), nameof(DateTime.AddDays), new DateTimeAddDaysTranslator());
            config.Markup.JavascriptTranslator.MethodCollection.AddPropertyGetterTranslator(typeof(DateTime), nameof(DateTime.DayOfYear), new DateTimeDayOfYearTranslator());
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Default", "", "Views/default.dothtml");

            // Uncomment the following line to auto-register all dothtml files in the Views folder
            // config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));    
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
            config.Markup.AddCodeControls("cc", typeof(DraggableList));
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.Register("jquery", new ScriptResource()
            {
                Location = new UrlResourceLocation("https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js")
            });

            config.Resources.Register("draggableList", new ScriptResource()
            {
                Location = new UrlResourceLocation("/Controls/DraggableList.js"),
                Dependencies = new[] { "jquery", "knockout" }
            });

            config.Resources.Register("utils", new ScriptResource()
            {
                Location = new UrlResourceLocation("/utils.js")
            });
        }
    }

}
