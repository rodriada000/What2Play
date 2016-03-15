using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameDecider.Startup))]
namespace GameDecider
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
