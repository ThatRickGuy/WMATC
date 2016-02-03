using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WMATC.Startup))]
namespace WMATC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
