using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReviewDownloader.Startup))]
namespace ReviewDownloader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
