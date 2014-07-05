

using EgoSignalR;
using Microsoft.Owin;
using Owin;
[assembly:OwinStartup(typeof(Startup))]
namespace EgoSignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}