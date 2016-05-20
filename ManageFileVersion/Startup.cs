using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManageFileVersion.Startup))]
namespace ManageFileVersion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
