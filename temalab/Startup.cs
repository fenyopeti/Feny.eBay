using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(temalab.Startup))]
namespace temalab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
