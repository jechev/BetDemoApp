using BetDemoApp.Data;
using Microsoft.Owin;
using Owin;
using Hangfire;
using BetDemoApp.Web.Services;

[assembly: OwinStartupAttribute(typeof(BetDemoApp.Web.Startup))]
namespace BetDemoApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var service = new DataReceiveService();
            using (var dbContext = new BetDemoAppDbContext())
            {
                dbContext.Database.Initialize(true);
                service.GetData();
            }
            
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            RecurringJob.AddOrUpdate(() => CallService(service), Cron.Minutely);

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            ConfigureAuth(app);

            app.MapSignalR();
        }

        public void CallService(DataReceiveService service)
        {
            service.GetData();
            SignalRService.Instance.UpdateAll();
        }
    }
}
