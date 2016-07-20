using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using RealTimeChat.Hubs;
using RealTimeChat.Models;
using RealTimeChat.Services;
using Unity.Mvc4;

[assembly: OwinStartupAttribute(typeof(RealTimeChat.Startup))]
namespace RealTimeChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //required for signalR setup
            app.MapSignalR();
        }
    }
}
