using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BookSheetMigratorInterface.Startup))]

namespace BookSheetMigratorInterface
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
