using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MRS.Startup))]
namespace MRS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
