using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MurongEnrollment.Startup))]
namespace MurongEnrollment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
