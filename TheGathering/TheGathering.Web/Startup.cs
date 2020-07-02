using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheGathering.Web.Startup))]
namespace TheGathering.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
