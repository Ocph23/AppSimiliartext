using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdrianaApp.Startup))]
namespace AdrianaApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
