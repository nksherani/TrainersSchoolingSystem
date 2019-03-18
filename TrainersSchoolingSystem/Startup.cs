using Hangfire;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(TrainersSchoolingSystem.Startup))]
namespace TrainersSchoolingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings[1].ConnectionString);
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
