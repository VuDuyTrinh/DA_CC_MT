using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DA_CCMT.Startup))]
namespace DA_CCMT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
