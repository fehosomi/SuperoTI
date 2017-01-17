using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SuperoTI.Task.Web.Startup))]
namespace SuperoTI.Task.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
