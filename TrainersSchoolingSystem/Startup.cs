using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainersSchoolingSystem.Startup))]
namespace TrainersSchoolingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
